using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<string> UserRoles { get; set; } = new List<string>();

}
