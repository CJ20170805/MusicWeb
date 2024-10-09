using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.Interfaces;

public interface INotificationService
{
    Task AddNotificationAsync(Notification notification);
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    Task<int> GetUnreadNotificationCountAsync(Guid userId);
}
