using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Domain.Entities;
using MusicApp.Application.DTOs;
using MediatR;
using MusicApp.Application.Commands;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediatR;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(IMediator mediator ,UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _mediatR = mediator;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // var user = new User
        // {
        //     UserName = registerDTO.Email,
        //     Email = registerDTO.Email,
        // };
        var command = new RegisterUserCommand
        {
            UserName = registerDTO.UserName,
            Email = registerDTO.Email,
            Password = registerDTO.Password
        };

        var result = await _mediatR.Send(command);
        if (result)
        {
            return Ok(new { message = "User registered successfully!" });
        }

        return BadRequest(new { message = "Failed to register user." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var token = GenerateJwtToken(user);
            // Sign the user in for cookie-based authentication
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (signInResult.Succeeded)
            {
                // Return JWT token for API requests
                return Ok(new { token });
            }
            return Unauthorized();
        }
        return Unauthorized();
    }

    private string GenerateJwtToken(User user)
    {
        var configKey = _configuration["Jwt:Key"];
        if (configKey == null)
        {
            throw new Exception("JWT Key not found in configuration");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));  // Encoding is now available
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
