using System;
using Microsoft.AspNetCore.Identity;

namespace MusicApp.Domain.Entities;

public class User: IdentityUser<Guid>
{
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    public virtual ICollection<Playlist> Playlists { get; private set; } 
    public virtual ICollection<Notification> Notifications { get; private set; }
    public virtual ICollection<UserRoles> UserRoles { get; private set; }
    public User()
    {
        // Initialize properties
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsDeleted = false;
        Playlists = new List<Playlist>();
        Notifications = new List<Notification>();
        UserRoles = new List<UserRoles>();
    }

    public void AddUserRole(UserRoles userRole)
    {
        // Add the new role to the collection
        UserRoles.Add(userRole);
    }

    public void RemoveUserRole(UserRoles userRole)
    {
        // Remove the role from the collection if it exists
        UserRoles.Remove(userRole);
    }

    public void ClearUserRoles()
    {
        // Clear all roles from the collection
        UserRoles.Clear();
    }


    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
    
}
