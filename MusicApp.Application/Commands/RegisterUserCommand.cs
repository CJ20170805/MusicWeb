using System;
using MediatR;

namespace MusicApp.Application.Commands;

public class RegisterUserCommand: IRequest<bool>
{
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
