using System;
using MediatR;

namespace MusicApp.Application.Events;

public class UserRegisteredEvent : INotification
{
    public Guid UserId {get; }
    public string UserEmail { get; }

    public UserRegisteredEvent(Guid userId, string userEmail)
    {
        UserEmail = userEmail;
        UserId = userId;
    }
}
