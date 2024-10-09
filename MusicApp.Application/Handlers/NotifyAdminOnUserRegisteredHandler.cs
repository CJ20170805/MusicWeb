using System;
using MediatR;
using MusicApp.Application.Events;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;

namespace MusicApp.Application.Handlers;

public class NotifyAdminOnUserRegisteredHandler: INotificationHandler<UserRegisteredEvent>
{
    private readonly INotificationRepository _notificationRepository;
    public NotifyAdminOnUserRegisteredHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    } 
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        // Create a notification object
        var newNotification = new Notification
        (
            $"New user registered: {notification.UserEmail}",
            notification.UserId
        );

        // Add the notification to the repository
        await _notificationRepository.AddNotificationAsync(newNotification);
    }

    
}
