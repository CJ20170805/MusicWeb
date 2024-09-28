using System;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Application.DTOs;

public class LoginDTO
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}