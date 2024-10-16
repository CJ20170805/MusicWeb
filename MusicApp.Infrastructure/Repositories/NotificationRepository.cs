using Microsoft.EntityFrameworkCore;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;
using MusicApp.Infrastructure.Data;

namespace MusicApp.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly MusicDbContext _context;
    public NotificationRepository(MusicDbContext context)
    {
        _context = context;
    }

    public async Task AddNotificationAsync(string message, IEnumerable<Guid> userIds)
    {
        var notification = new Notification(message);
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();


        var userNotifications = userIds.Select(userId => new UserNotificationRead(userId, notification.Id)).ToList();
        await _context.UserNotificationReads.AddRangeAsync(userNotifications);
        await _context.SaveChangesAsync();
    }

    public async Task MarkNotificationAsReadAsync(Guid userId, List<string> notificationIds)
    {
        var notifications = await _context.UserNotificationReads
            .Where(unr => unr.UserId == userId && notificationIds.Contains(unr.NotificationId.ToString()) && !unr.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.MarkAsRead();
        }
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetNotificationCountByUserIdAsync(Guid userId)
    {
        return await _context.UserNotificationReads
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync();
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
    {
        return await _context.UserNotificationReads
                .Where(n => n.UserId == userId && !n.IsRead && n.Notification != null)
                .Select(n => n.Notification!)
                .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByRoleAsync(Guid userId, bool isAdmin)
    {
        if (isAdmin)
        {
            // Fetch notifications for all admins or specific admin
            return await _context.Notifications
                //.Where(n =>  n.UserId == userId)
                .Where(n => n.UserNotificationReads.Any(ur => ur.UserId == userId && !ur.IsRead))
                .ToListAsync();
        }
        else
        {
            // Fetch notifications for all users or specific user
            return await _context.Notifications
                //.Where(n => n.IsForUser || n.UserId == userId)
                .Where(n => n.UserNotificationReads.Any(ur => ur.UserId == userId && !ur.IsRead))
                .ToListAsync();
        }
    }

}
