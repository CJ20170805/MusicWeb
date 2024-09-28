using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Domain.Interfaces;

public interface ITrackRepository
{
    Task<Track> GetTrackByIdAsync(Guid trackId);
    Task<IEnumerable<Track>> GetAllTracksAsync();
    Task AddTrackAsync(Track track);
    Task UpdateTrackAsync(Track track);
    Task DeleteTrackAsync(Guid trackId);
}
