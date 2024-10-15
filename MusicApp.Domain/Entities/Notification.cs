using System;

namespace MusicApp.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public string? Message { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsRead { get; private set; }
    public bool IsForAdmin { get; private set; }
    public bool IsForUser { get; private set; }
    public Guid? UserId { get;  set; }
    // public User? User { get; private set; }
    public Notification(string message, bool isForAdmin, bool isForUser, Guid? userId)
    {
        Id = Guid.NewGuid();
        Message = message;
        CreatedAt = DateTime.UtcNow;
        IsRead = false;
        IsForAdmin = isForAdmin;
        IsForUser = isForUser;
        UserId = userId ?? Guid.Empty;
    }
}
