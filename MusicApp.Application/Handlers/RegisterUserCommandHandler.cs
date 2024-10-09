using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MusicApp.Domain.Entities;
using MusicApp.Application.Commands;
using MusicApp.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MusicApp.Application.Handlers;

public class RegisterUserCommandHandler: IRequestHandler<RegisterUserCommand, bool>
{
   private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(UserManager<User> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            // Send confirmation email after successful registration
            await _emailService.SendEmailAsync(user.Email, "Welcome to MusicApp", "Thank you for registering!");

            return true;
        }

        return false;
    }
}