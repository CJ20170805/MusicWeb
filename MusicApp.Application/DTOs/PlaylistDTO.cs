using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.DTOs;

public class PlaylistDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public Guid UserId { get; set; }
    public UserDTO? User { get; set; }
    public List<TrackDTO>? Tracks { get; set; } 
    public DateTime CreatedAt { get; set; }
}
