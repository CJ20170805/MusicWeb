using AutoMapper;
using MusicApp.Application.DTOs;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;

namespace MusicApp.Application.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMapper _mapper;

        public PlaylistService(IPlaylistRepository playlistRepository, IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _mapper = mapper;
        }

        public async Task<PlaylistDTO> GetPlaylistByIdAsync(Guid playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
            return _mapper.Map<PlaylistDTO>(playlist);
        }

        public async Task<IEnumerable<PlaylistDTO>> GetPlaylistByUserIdAsync(Guid userId)
        {
            var playlists = await _playlistRepository.GetAllPlaylistsAsync(userId);
            return _mapper.Map<IEnumerable<PlaylistDTO>>(playlists);
        }

        public async Task<Guid> CreatePlaylistAsync(PlaylistDTO playlistDTO)
        {
            var playlist = _mapper.Map<Playlist>(playlistDTO);
            await _playlistRepository.AddPlaylistAsync(playlist);
            return playlist.Id;
        }

        public async Task<bool> UpdatePlaylistAsync(PlaylistDTO playlistDTO)
        {
            var playlist = _mapper.Map<Playlist>(playlistDTO);
            await _playlistRepository.UpdatePlaylistAsync(playlist);
            return true;
        }

        public async Task<bool> DeletePlaylistAsync(Guid playlistId)
        {
            await _playlistRepository.DeletePlaylistAsync(playlistId);
            return true;
        }
    }
}
