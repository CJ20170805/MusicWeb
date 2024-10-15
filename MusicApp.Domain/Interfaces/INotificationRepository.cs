using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Domain.Interfaces;

public interface INotificationRepository
{
    Task AddNotificationAsync(Notification notification);
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    Task<int> GetNotificationCountByUserIdAsync(Guid userId);
    Task<IEnumerable<Notification>> GetNotificationsByRoleAsync(Guid userId, bool isAdmin);
}
