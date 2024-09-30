using System;
using MusicApp.Application.DTOs;

namespace MusicApp.Application.Interfaces;

public interface ITrackService
{
    Task<TrackDTO> GetTrackByIdAsync(Guid trackId);
    Task<IEnumerable<TrackDTO>> GetTracksAllAsync();
    Task<Guid> CreateTrackAsync(TrackDTO trackDTO);
    Task<bool> UpdateTrackAsync(TrackDTO trackDTO);
    Task<bool> DeleteTrackAsync(Guid trackId);
}
