using System;

namespace MusicApp.Application.DTOs;

public class UserRolesDTO
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public string? RoleName { get; set; }

}
