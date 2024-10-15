using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.Hubs;

public class NotificationHub: Hub
{
    private readonly UserManager<User> _userManager; 
    public NotificationHub(UserManager<User> userManager)
    {
        _userManager = userManager; 
    }
    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }

    public override async Task OnConnectedAsync()
    {
        var user = Context.User;

        if (user?.Identity is not null && user.Identity.IsAuthenticated)
        {
            Console.WriteLine($"User connected: {Context.User.Identity?.Name}, Connection ID: {Context.ConnectionId}, Admin? {Context.User.IsInRole("Admin")}, User? {Context.User.IsInRole("User")}, XXXL:{Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");

            if (Context.User.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }
            else if (Context.User.IsInRole("User"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Users");
            }
        }
        else
        {
            Console.WriteLine($"Anonymous user connected. Connection ID: {Context.ConnectionId}");
        }
        
        await base.OnConnectedAsync();
    }
}
