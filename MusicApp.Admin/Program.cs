using MusicApp.Admin.Components;
using MusicApp.Application.Services;
using MusicApp.Application.Interfaces;
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

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found."));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Optional: to avoid clock skew issues
    };
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "jwt"; // Set the cookie name for authentication
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Secure cookies over HTTPS
    options.LoginPath = "/Account/Login"; // Adjust login path
    options.LogoutPath = "/Account/Logout"; // Adjust logout path
});

builder.Services.AddMudServices();

// AutoMapper setup
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

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

// Dependency Injection for application services
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IUserService, UserService>();

// Dependency Injection for repository services
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

var app = builder.Build();

// Configure the HTTP request pipeline for Blazor Server
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Razor Components (Blazor Server)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
