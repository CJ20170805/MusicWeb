using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MusicApp.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MusicApp.Application.Hubs;

public class NotificationHub: Hub
{
    private readonly UserManager<User> _userManager; 
    private readonly IHttpContextAccessor _httpContextAccessor;
    public NotificationHub(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager; 
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }

     public async Task AddToGroup(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            throw new ArgumentException("Group name cannot be null or empty.", nameof(groupName));
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        Console.WriteLine($"Connection {Context.ConnectionId} added to group {groupName}.");
    }

    // public override async Task OnConnectedAsync()
    // {
    //     var user = Context.User;

    //     if (user?.Identity is not null && user.Identity.IsAuthenticated)
    //     {
    //         Console.WriteLine($"User connected: {Context.User.Identity?.Name}, Connection ID: {Context.ConnectionId}, Admin? {Context.User.IsInRole("Admin")}, User? {Context.User.IsInRole("User")}, XXXL:{Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");

    //         if (Context.User.IsInRole("Admin"))
    //         {
    //             await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
    //         }
    //         else if (Context.User.IsInRole("User"))
    //         {
    //             await Groups.AddToGroupAsync(Context.ConnectionId, "Users");
    //         }
    //     }
    //     else
    //     {
    //         Console.WriteLine($"Anonymous user connected. Connection ID: {Context.ConnectionId}");
    //     }

    

    //     await base.OnConnectedAsync();
    // }
}
