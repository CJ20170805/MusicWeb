using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> AddUserAsync(User user, string password);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
}
