using System;
using Microsoft.AspNetCore.Identity;

namespace MusicApp.Infrastructure.Data;

public class RoleInitializer
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RoleInitializer(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task InitializeRolesAsync()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
        }
    }
}
