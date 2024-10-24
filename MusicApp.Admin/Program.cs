using FluentValidation;
using MusicApp.Admin.Components;
using MusicApp.Application.Services;
using MusicApp.Application.Interfaces;
using MusicApp.Application.DTOs;
using MusicApp.Infrastructure.Repositories;
using MusicApp.Domain.Interfaces;
using MusicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MusicApp.Application.Mappings;
using MudBlazor.Services;
using MusicApp.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using MusicApp.Admin.Services;
using MusicApp.Application.Commands;
using MusicApp.Application.Handlers;
using MediatR;
using MusicApp.Application.Hubs;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5279")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials(); 
        });
});

builder.Services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);



var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found."));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; 
})
.AddCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
    options.LoginPath = "/Account/Login"; 
    options.LogoutPath = "/Account/Logout"; 
});

builder.Services.AddMudServices();

builder.Services.AddSingleton<ThemeService>();

// Register FluentValidation
builder.Services.AddTransient<IValidator<LoginDTO>, LoginDTOValidator>();


// AutoMapper setup
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();

// Set up the HTTP client for API calls
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5110/"); // Replace with your actual API base URL
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

// Add Razor Components for Blazor Server and MudBlazor services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// Register MediatR services
builder.Services.AddMediatR(typeof(RegisterUserCommandHandler).Assembly);

// DI for Email service
builder.Services.AddScoped<IEmailService, EmailServices>();

// Dependency Injection for application services
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

// Dependency Injection for repository services
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});



// MySQL Database setup
var serverVersion = new MySqlServerVersion(new Version(8, 2, 0));
builder.Services.AddDbContextFactory<MusicDbContext>(dbContextOptions => 
    dbContextOptions.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."), serverVersion)
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

// ASP.NET Identity setup (for managing users)
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<MusicDbContext>()
.AddDefaultTokenProviders();

// Register the RoleInitializer
builder.Services.AddTransient<RoleInitializer>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleInitializer = services.GetRequiredService<RoleInitializer>();

    // Initialize roles 
    await roleInitializer.InitializeRolesAsync();
}

// Configure the HTTP request pipeline for Blazor Server
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAntiforgery();



// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/NotificationHub");


// Map Razor Components (Blazor Server)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();
