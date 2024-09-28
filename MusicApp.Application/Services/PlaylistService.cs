using System;
using MusicApp.Application.DTOs;
using MusicApp.Application.Interfaces;

namespace MusicApp.Application.Services;

public class PlaylistService : IPlaylistService
{
    public Task<Guid> CreatePlaylistAsync(PlaylistDTO playlistDTO)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeletePlaylistAsync(Guid playlistId)
    {
        throw new NotImplementedException();
    }

    public Task<PlaylistDTO> GetPlaylistByIdAsync(Guid playlistId)
    {
        throw new NotImplementedException();
    }

    public Task<List<PlaylistDTO>> GetPlaylistByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdatePlaylistAsync(PlaylistDTO playlistDTO)
    {
        throw new NotImplementedException();
    }
}
