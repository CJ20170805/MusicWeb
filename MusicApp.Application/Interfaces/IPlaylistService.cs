using System;
using MusicApp.Application.DTOs;

namespace MusicApp.Application.Interfaces;

public interface IPlaylistService
{
    Task<PlaylistDTO> GetPlaylistByIdAsync(Guid playlistId);
    Task<List<PlaylistDTO>> GetPlaylistByUserIdAsync(int userId);
    Task<Guid> CreatePlaylistAsync(PlaylistDTO playlistDTO);
    Task<bool> UpdatePlaylistAsync(PlaylistDTO playlistDTO);
    Task<bool> DeletePlaylistAsync(Guid playlistId);
}
