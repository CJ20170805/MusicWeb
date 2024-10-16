using System;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Interfaces;
using MusicApp.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using MusicApp.Application.Hubs;
using MusicApp.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MusicApp.Application.Services;

public class NotificationService: INotificationService
{
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        public NotificationService(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext, IMediator mediator, UserManager<User> userManager)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task AddNotificationAsync(string message, IEnumerable<Guid> userIds)
        {
            await _notificationRepository.AddNotificationAsync(message, userIds);
        }
        public async Task MarkNotificationAsReadAsync(Guid userId, List<string> notificationIds)
        {
            await _notificationRepository.MarkNotificationAsReadAsync(userId, notificationIds);
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

        // Test methods
        public async Task<bool> TestSendToAllAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            foreach (var admin in admins)
            {
               Console.WriteLine($"Admin: {admin.UserName} Id: {admin.Id}");
            }
            var adminIds = admins.Select(a => a.Id).ToList();


            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"Test Message MMMM: ({formattedDate})");
            string msg = $"Test Message(All admins): ({formattedDate})";
            await SaveNotificationToDB(msg, adminIds);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", msg);
            return true;  

        }
        public async Task<bool> TestSendToAllUsers()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");
            foreach (var user in users)
            {
               Console.WriteLine($"User: {user.UserName} Id: {user.Id}");
            }
            var userIds = users.Select(u => u.Id).ToList();

            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"Test Message MMMM: ({formattedDate})");
            string msg = $"Test Message(All users): ({formattedDate})";
            await SaveNotificationToDB(msg, userIds);
            await _hubContext.Clients.Group("Users").SendAsync("ReceiveNotification", msg);
            return true;  
        }
        public async Task<bool> TestSendToAUser(IEnumerable<Guid> userIds)
        {
            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"Test Message MMMM: ({formattedDate})");
            string msg = $"Test Message(A user): ({formattedDate})";
            await SaveNotificationToDB(msg,userIds);
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", msg);
            }
            return true;  
        }
        private async Task<bool> SaveNotificationToDB(string message, IEnumerable<Guid> userIds)
        {
            await _notificationRepository.AddNotificationAsync(message, userIds);
            return true;
        }
}
