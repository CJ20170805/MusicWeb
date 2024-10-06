using System;

namespace MusicApp.Domain.Entities;

public class Playlist
{
    public Guid Id { get; private set; }
    public string? Title { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; } 

    public ICollection<PlaylistTrack> PlaylistTracks { get; private set; }

    public Playlist(string title, Guid userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        PlaylistTracks = new List<PlaylistTrack>();
    }

    public void AddTrack(Track track)
    {
        if(PlaylistTracks == null)
        {
            PlaylistTracks = new List<PlaylistTrack>();
        }

        if(!PlaylistTracks.Any(pt => pt.TrackId == track.Id))
        {
            PlaylistTracks.Add(new PlaylistTrack
            {
                PlaylistId = Id,
                TrackId = track.Id,
                Playlist = this,
                Track = track
            });

            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemoveTrack(Track track)
    {
        if (track == null) throw new ArgumentNullException(nameof(track));
        var playlistTrack = PlaylistTracks.FirstOrDefault(pt => pt.TrackId == track.Id);
        if (playlistTrack != null)
        {
            PlaylistTracks.Remove(playlistTrack);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void UpdateTitle(string newTitle)
    {
        Title = newTitle;
        UpdatedAt = DateTime.UtcNow;
    }
}
