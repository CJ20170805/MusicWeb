using System;

namespace MusicApp.Application.Interfaces;
using MusicApp.Application.DTOs;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetUsersAllAsync();
    Task<bool> UpdateUserSAsync(UserDTO userDTO);
    Task<bool> DeleteUserSAsync(Guid id);
}
