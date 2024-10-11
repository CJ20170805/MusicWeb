using System;
using MusicApp.Application.DTOs;

namespace MusicApp.Application.Interfaces;

public interface IUserRoleService
{
    Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
    Task AssignRoleToUserAsync(Guid userId, IEnumerable<string> roles);
    Task<string> GetRoleNameByIdAsync(Guid roleId);
}
