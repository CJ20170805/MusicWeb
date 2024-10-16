using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicApp.Domain.Entities;

namespace MusicApp.Infrastructure.Data;

public class MusicDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options)
    {

    }

    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserNotificationRead> UserNotificationReads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicDbContext).Assembly);


        //Configure PlaylistTrack many-to-many relationship
        modelBuilder.Entity<PlaylistTrack>()
            .HasKey(pt => new { pt.PlaylistId, pt.TrackId });

        modelBuilder.Entity<PlaylistTrack>()
            .HasOne(pt => pt.Playlist)
            .WithMany(p => p.PlaylistTracks)
            .HasForeignKey(pt => pt.PlaylistId);

        modelBuilder.Entity<PlaylistTrack>()
            .HasOne(pt => pt.Track)
            .WithMany(t => t.PlaylistTracks)
            .HasForeignKey(pt => pt.TrackId);

       

       // Configure UserNotificationRead relationship
        modelBuilder.Entity<UserNotificationRead>()
            .HasKey(unr => new { unr.UserId, unr.NotificationId });

        modelBuilder.Entity<UserNotificationRead>()
            .HasOne(unr => unr.User)
            .WithMany(u => u.UserNotificationReads)
            .HasForeignKey(unr => unr.UserId);
        
        modelBuilder.Entity<UserNotificationRead>()
            .HasOne(unr => unr.Notification)
            .WithMany(n => n.UserNotificationReads)
            .HasForeignKey(unr => unr.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
        

    }
}
