using System;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Application.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "The Username field is required.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Password field is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}