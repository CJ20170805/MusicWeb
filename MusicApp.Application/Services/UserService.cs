using System;
using AutoMapper;
using MusicApp.Application.DTOs;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Interfaces;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDTO>> GetUsersAllAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        if (users == null || !users.Any())
        {
            throw new Exception("No users found");
        }
        return _mapper.Map<IEnumerable<UserDTO>>(users);
    }

    public async Task<bool> UpdateUserSAsync(UserDTO userDTO)
    {
        if (userDTO == null)
        {
            throw new ArgumentNullException(nameof(userDTO));
        }
        var user = _mapper.Map<User>(userDTO);
        await _userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task<bool> DeleteUserSAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }
        await _userRepository.DeleteUserAsync(id);
        return true;
    }
}
