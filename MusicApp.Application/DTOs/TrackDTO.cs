using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.DTOs;

public class TrackDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public TimeSpan Duration { get; set; }
    public Guid? AudioFileId { get; set; }
    public FileUpload? AudioFile { get; set; }
    public Guid? CoverImageId { get; set; }
    public FileUpload? CoverImage { get; set; }
    
}
