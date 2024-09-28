using System;
using Microsoft.AspNetCore.Identity;

namespace MusicApp.Domain.Entities;

public class User: IdentityUser<Guid>
{
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    public virtual ICollection<Playlist> Playlists { get; private set; } 
    public User()
    {
        // Initialize properties
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsDeleted = false;
        Playlists = new List<Playlist>();
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
    
}
