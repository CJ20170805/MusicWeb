using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicApp.Application.DTOs;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.Services;

public class UserRoleService : IUserRoleService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserRoleService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Select(role => new RoleDTO
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty
        }).ToList();
    }

    public async Task<string> GetRoleNameByIdAsync(Guid roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null) throw new InvalidOperationException("Role not found");

        return role.Name ?? string.Empty;
    }

    public async Task AssignRoleToUserAsync(Guid userId, IEnumerable<string> roles)
    {
        Console.WriteLine("RRR2" + userId + string.Join(", ", roles));
        // Find the user by userId
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new InvalidOperationException("User not found");

        // Retrieve the roles currently assigned to the user
        var existingRoles = await _userManager.GetRolesAsync(user);

        foreach (var role in existingRoles)
        {
            Console.WriteLine($"Existing role: {role}");
        }

        // Determine roles to add (roles that are in the 'roles' parameter but not in the existing roles)
        var rolesToAdd = roles.Except(existingRoles).ToList();

        // Determine roles to remove (roles that are in the existing roles but not in the 'roles' parameter)
        var rolesToRemove = existingRoles.Except(roles).ToList();

        // Add new roles
        if (rolesToAdd.Any())
        {
            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to assign roles: " + string.Join(", ", rolesToAdd));
            }
            Console.WriteLine($"Added roles: {string.Join(", ", rolesToAdd)}");
        }

        // Remove old roles
        if (rolesToRemove.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to remove roles: " + string.Join(", ", rolesToRemove));
            }
            Console.WriteLine($"Removed roles: {string.Join(", ", rolesToRemove)}");
        }

        // Optionally update the user in case other changes are made
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            throw new InvalidOperationException("Failed to update user.");
        }

        Console.WriteLine($"Successfully updated roles for user: {user.UserName}");

    }
}

