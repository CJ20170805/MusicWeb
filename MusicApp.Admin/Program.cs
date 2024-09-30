using MusicApp.Admin.Components;
using MusicApp.Application.Services;
using MusicApp.Application.Interfaces;
using MusicApp.Infrastructure.Repositories;
using MusicApp.Domain.Interfaces;
using MusicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper setup
// builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Inject application services
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

// Inject repository services
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
