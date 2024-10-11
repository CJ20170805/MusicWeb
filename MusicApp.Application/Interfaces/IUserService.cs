using System;

namespace MusicApp.Application.Interfaces;
using MusicApp.Application.DTOs;
using MusicApp.Domain.Entities;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetUsersAllAsync();
    Task<bool> UpdateUserSAsync(UserDTO userDTO);
    Task<bool> DeleteUserSAsync(Guid id);
    Task<User> CreateUserSAsync(RegisterDTO registerDTO, string password);
    Task<bool> RegisterUserAsync(RegisterDTO registerDTO);
}
