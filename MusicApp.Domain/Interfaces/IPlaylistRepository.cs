using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Domain.Interfaces;

public interface IPlaylistRepository
{
    Task<Playlist> GetPlaylistByIdAsync(Guid playlistId);
    Task<IEnumerable<Playlist>> GetAllPlaylistsAsync(Guid userId);
    Task AddPlaylistAsync(Playlist playlist);
    Task UpdatePlaylistAsync(Playlist playlist);
    Task DeletePlaylistAsync(Guid playlistId);
}
