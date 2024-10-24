using System.Text;
using Microsoft.EntityFrameworkCore;
using MusicApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using MusicApp.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Application.Interfaces;
using MusicApp.Application.Services;
using MusicApp.Infrastructure.Repositories;
using MusicApp.Domain.Interfaces;
using System.Security.Cryptography;
using MusicApp.Application.Mappings;
using MusicApp.Application.Handlers;
using MediatR;
using MusicApp.Application.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


builder.Services.AddSignalR();


// Add services to the container.
builder.Services.AddControllers();

// AutoMapper setup
// builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Register MediatR services
builder.Services.AddMediatR(typeof(RegisterUserCommandHandler).Assembly);

// DI for Email service
builder.Services.AddScoped<IEmailService, EmailServices>();

// DI for Notification service
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// DI for Playlist and Track services
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<ITrackService, TrackService>();

// DI for Playlist and Track repositories
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();

// DI for File service and repository
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();

// MySQL Database setup
var serverVersion = new MySqlServerVersion(new Version(8, 2, 0));
builder.Services.AddDbContextFactory<MusicDbContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."), serverVersion)
        // The following three options help with d ebugging, but should
        // be changed or removed for production.
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

// ASP.NET Identity setup
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => 
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<MusicDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication setup
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
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

 app.UseCors("AllowAll"); 

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();  
app.UseAuthorization();

app.MapHub<NotificationHub>("/NotificationHub");

app.MapGet("/GenerateRandomKey", () =>
{
    using (var hmac = new HMACSHA256())
    {
        var key = hmac.Key;
        return Convert.ToBase64String(key); // Store this key securely!
    }
});

app.Run();

