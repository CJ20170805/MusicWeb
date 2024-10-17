using System;

namespace MusicApp.Domain.Entities;

public class Track
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Artist { get; private set; }
    public TimeSpan Duration { get; private set; }

    // Foreign key to the cover image file
    public Guid? CoverImageId { get; private set; }
    public FileUpload? CoverImage { get; private set; }

    // Foreign key to the audio file
    public Guid? AudioFileId { get; private set; }
    public FileUpload? AudioFile { get; private set; }

    public ICollection<PlaylistTrack> PlaylistTracks { get; private set; }

    public Track(string title, string artist, TimeSpan duration)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title), "Title cannot be null or empty.");
        }
        
        if (string.IsNullOrWhiteSpace(artist))
        {
            throw new ArgumentNullException(nameof(artist), "Artist cannot be null or empty.");
        }
        
        Id = Guid.NewGuid();
        Title = title;
        Artist = artist;
        Duration = duration;
        PlaylistTracks = new List<PlaylistTrack>();
    }

    public void UpdateTrackInfo(string title, string artist, TimeSpan duration)
    {
        Title = title;
        Artist = artist;
        Duration = duration;
    }

    public void SetCoverImage(FileUpload coverImage)
    {
        CoverImage = coverImage;
        CoverImageId = coverImage.Id;
    }

    public void SetAudioFile(FileUpload audioFile)
    {
        AudioFile = audioFile;
        AudioFileId = audioFile.Id;
    }

}
