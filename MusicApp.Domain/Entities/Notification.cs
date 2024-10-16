using System;

namespace MusicApp.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; private set; }
    public ICollection<UserNotificationRead> UserNotificationReads { get; private set; } = new List<UserNotificationRead>();
    
    public Notification(string message)
    {
        Id = Guid.NewGuid();
        Message = message;
        CreatedAt = DateTime.UtcNow;
    }
}
