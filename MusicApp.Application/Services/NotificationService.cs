using System;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Interfaces;
using MusicApp.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using MusicApp.Application.Hubs;
using MusicApp.Application.Events;
using MediatR;

namespace MusicApp.Application.Services;

public class NotificationService: INotificationService
{
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMediator _mediator;
        public NotificationService(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext, IMediator mediator)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _mediator = mediator;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return await _notificationRepository.GetNotificationsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByRoleAsync(Guid userId, bool isAdmin)
        {
            return await _notificationRepository.GetNotificationsByRoleAsync(userId, isAdmin);
        }

        public async Task<int> GetUnreadNotificationCountAsync(Guid userId)
        {
            return await _notificationRepository.GetNotificationCountByUserIdAsync(userId);
        }

        // Test method
        public async Task<bool> TestNotification()
        {   
            // string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // Console.WriteLine($"Test Message MMMM: ({formattedDate})");
            // await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Test Message: ({formattedDate})");
            // return true;  

            // Publish user registered event
            var userRegisteredEvent = new UserRegisteredEvent(Guid.Parse("08dcea3b-6873-4393-8049-ac90017d7c56"), "Hahah@haha.com");
            await _mediator.Publish(userRegisteredEvent);  
            return true;
        }
        public async Task<bool> TestSendToAllAdmins()
        {
            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"Test Message MMMM: ({formattedDate})");
            string msg = $"Test Message(All admins): ({formattedDate})";
            await SaveNotificationToDB(msg, true, false);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", msg);
            return true;  

        }
        public async Task<bool> TestSendToAllUsers()
        {
            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"Test Message MMMM: ({formattedDate})");
            string msg = $"Test Message(All users): ({formattedDate})";
            await SaveNotificationToDB(msg, false, true);
            await _hubContext.Clients.Group("Users").SendAsync("ReceiveNotification", msg);
            return true;  
        }
        public async Task<bool> TestSendToAUser(IEnumerable<Guid> userIds)
        {
            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"Test Message MMMM: ({formattedDate})");
            string msg = $"Test Message(A user): ({formattedDate})";
            foreach (var userId in userIds)
            {
                await SaveNotificationToDB(msg, false, false, userId);
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", msg);
            }
            return true;  
        }
        private async Task<bool> SaveNotificationToDB(string message, bool isForAdmin, bool isForUser, Guid? userId=null)
        {
            var newNotification = new Notification
            (
                message,
                isForAdmin,
                isForUser,
                userId
            );

            // Add the notification to the repository
            await _notificationRepository.AddNotificationAsync(newNotification);
            return true;
        }
    }
