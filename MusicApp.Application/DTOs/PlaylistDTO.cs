using System;

namespace MusicApp.Application.DTOs;

public class PlaylistDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public int? UserId { get; set; }
    public List<TrackDTO>? Tracks { get; set; } 
}
