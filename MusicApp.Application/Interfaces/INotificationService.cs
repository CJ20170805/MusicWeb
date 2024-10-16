using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.Interfaces;

public interface INotificationService
{
    Task AddNotificationAsync(string message, IEnumerable<Guid> userIds);
    Task MarkNotificationAsReadAsync(Guid userId, List<string> notificationIds);
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    Task<IEnumerable<Notification>> GetNotificationsByRoleAsync(Guid userId, bool isAdmin);
    Task<int> GetUnreadNotificationCountAsync(Guid userId);
    Task<bool> TestSendToAllAdmins();
    Task<bool> TestSendToAllUsers();
    Task<bool> TestSendToAUser(IEnumerable<Guid> userIds);
}
