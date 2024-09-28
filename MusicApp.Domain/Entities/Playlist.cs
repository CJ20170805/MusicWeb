using System;

namespace MusicApp.Domain.Entities;

public class Playlist
{
    public Guid Id { get; private set; }
    public string? Title { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public List<Track>? Tracks { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; } 

    public ICollection<PlaylistTrack> PlaylistTracks { get; private set; }

    public Playlist(string title, Guid userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        UserId = userId;
        Tracks = new List<Track>();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        PlaylistTracks = new List<PlaylistTrack>();
    }

    public void AddTrack(Track track)
    {
        Tracks?.Add(track);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveTrack(Track track)
    {
        Tracks?.Remove(track);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTitle(string newTitle)
    {
        Title = newTitle;
        UpdatedAt = DateTime.UtcNow;
    }
}
