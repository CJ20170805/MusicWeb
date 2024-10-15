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

    public async Task AddNotificationAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetNotificationCountByUserIdAsync(Guid userId)
    {
        return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync();
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
    {
        return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByRoleAsync(Guid userId, bool isAdmin)
    {
        if (isAdmin)
        {
            // Fetch notifications for all admins or specific admin
            return await _context.Notifications
                .Where(n => n.IsForAdmin || n.UserId == userId)
                .ToListAsync();
        }
        else
        {
            // Fetch notifications for all users or specific user
            return await _context.Notifications
                .Where(n => n.IsForUser || n.UserId == userId)
                .ToListAsync();
        }
    }

}
