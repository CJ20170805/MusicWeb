using System;

namespace MusicApp.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public string? Message { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsRead { get; private set; }
    public Guid UserId { get;  set; }
    public User? User { get; private set; }
    public Notification(string message, Guid userId)
    {
        Id = Guid.NewGuid();
        Message = message;
        CreatedAt = DateTime.UtcNow;
        IsRead = false;
        UserId = userId;
    }
}
