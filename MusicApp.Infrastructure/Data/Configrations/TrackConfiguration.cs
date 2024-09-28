using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.Domain.Entities;

namespace MusicApp.Infrastructure.Data.Configrations;

public class TrackConfiguration: IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title).HasMaxLength(150).IsRequired();

        // builder.Property(t => t.Artist).IsRequired().HasMaxLength(100);

        builder.HasMany(t => t.PlaylistTracks)
            .WithOne(pt => pt.Track)
            .HasForeignKey(pt => pt.TrackId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}