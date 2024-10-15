using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.Interfaces;

public interface INotificationService
{
    Task AddNotificationAsync(Notification notification);
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    Task<IEnumerable<Notification>> GetNotificationsByRoleAsync(Guid userId, bool isAdmin);
    Task<int> GetUnreadNotificationCountAsync(Guid userId);
    Task<bool> TestNotification();
    Task<bool> TestSendToAllAdmins();
    Task<bool> TestSendToAllUsers();
    Task<bool> TestSendToAUser(Guid userId);
}
