using System;
using AutoMapper;
using MusicApp.Application.DTOs;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Interfaces;
using MusicApp.Domain.Entities;
using MusicApp.Application.Commands;
using MediatR;

namespace MusicApp.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public UserService(IUserRepository userRepository, IMapper mapper, IMediator mediator)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<bool> RegisterUserAsync(RegisterDTO registerDTO)
    {
        var command = new RegisterUserCommand
        {
            UserName = registerDTO.UserName,
            Email = registerDTO.Email,
            Password = registerDTO.Password
        };

        return await _mediator.Send(command);
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

    public async Task<User> CreateUserSAsync(RegisterDTO registerDTO, string password)
    {
        if (registerDTO == null)
        {
            throw new ArgumentNullException(nameof(registerDTO));
        }
        // var user = _mapper.Map<User>(userDTO);
        var user = new User
        {
            UserName = registerDTO.UserName,
            Email = registerDTO.Email,
        };

        Console.WriteLine("3333" + user.Email + user.UserName);

        // foreach (var item in user.UserRoles)
        // {
        //     Console.WriteLine("5555" +item.RoleId);
        // }
        var ruser = await _userRepository.AddUserAsync(user, password);
        return ruser;
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
