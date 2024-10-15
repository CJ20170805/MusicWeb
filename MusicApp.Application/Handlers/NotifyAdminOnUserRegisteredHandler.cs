using System;
using MediatR;
using MusicApp.Application.Events;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using MusicApp.Application.Hubs;

namespace MusicApp.Application.Handlers;

public class NotifyAdminOnUserRegisteredHandler: INotificationHandler<UserRegisteredEvent>
{
    private readonly INotificationRepository _notificationRepository;
     private readonly IHubContext<NotificationHub> _hubContext;
    public NotifyAdminOnUserRegisteredHandler(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext)
    {
        _notificationRepository = notificationRepository;
        _hubContext = hubContext;
    }
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        // Create a notification object
        var newNotification = new Notification
        (
            $"New user registered: {notification.UserEmail}",
            true,
            false,
            null
        );

        // Add the notification to the repository
        await _notificationRepository.AddNotificationAsync(newNotification);

        // Notify admin users via SignalR
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"New user registered: {notification.UserId} ({notification.UserEmail})");
            
    }

    
}
