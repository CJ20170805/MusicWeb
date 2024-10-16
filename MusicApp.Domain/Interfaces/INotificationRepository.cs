using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Domain.Interfaces;

public interface INotificationRepository
{
    Task AddNotificationAsync(string message, IEnumerable<Guid> userIds);
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    Task<int> GetNotificationCountByUserIdAsync(Guid userId);
    Task<IEnumerable<Notification>> GetNotificationsByRoleAsync(Guid userId, bool isAdmin);
    Task MarkNotificationAsReadAsync(Guid userId, List<string> notificationIds);
}
