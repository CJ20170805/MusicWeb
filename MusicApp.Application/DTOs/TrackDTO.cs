using System;

namespace MusicApp.Application.DTOs;

public class TrackDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public TimeSpan Duration { get; set; }
    public Guid? PlaylistId { get; set; }
}
