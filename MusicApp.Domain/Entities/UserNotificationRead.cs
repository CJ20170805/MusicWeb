using System;

namespace MusicApp.Domain.Entities;

public class UserNotificationRead
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid NotificationId { get; set; }
    public Notification? Notification { get; set; }
    public bool IsRead { get; set; }
    public DateTime ReadAt { get; private set; }

    public UserNotificationRead(Guid userId, Guid notificationId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        NotificationId = notificationId;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
    }
}
