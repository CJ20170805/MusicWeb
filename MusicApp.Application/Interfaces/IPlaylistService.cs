using System;
using MusicApp.Application.DTOs;

namespace MusicApp.Application.Interfaces;

public interface IPlaylistService
{
    Task<PlaylistDTO> GetPlaylistByIdAsync(Guid playlistId);
    Task<IEnumerable<PlaylistDTO>> GetPlaylistByUserIdAsync(Guid userId);
    Task<IEnumerable<PlaylistDTO>> GetPlaylistsAllAsync();
    Task<Guid> CreatePlaylistAsync(PlaylistDTO playlistDTO);
    Task<bool> UpdatePlaylistAsync(PlaylistDTO playlistDTO);
    Task<bool> DeletePlaylistAsync(Guid playlistId);
    Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId);
    Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId);
}
