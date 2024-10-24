using System;
using System.ComponentModel.DataAnnotations;


namespace MusicApp.Application.DTOs;

public class RegisterDTO
{
    [Required]
    [StringLength(20, MinimumLength = 2)]
    public required string UserName { get; set; } 
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }
}


