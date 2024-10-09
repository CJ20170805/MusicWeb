using System;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Interfaces;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.Services;

public class NotificationService: INotificationService
{
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return await _notificationRepository.GetNotificationsByUserIdAsync(userId);
        }

        public async Task<int> GetUnreadNotificationCountAsync(Guid userId)
        {
            return await _notificationRepository.GetNotificationCountByUserIdAsync(userId);
        }
    }
