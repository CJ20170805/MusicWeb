using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicApp.Application.DTOs;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;

namespace MusicApp.Application.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ITrackRepository _trackRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaylistService> _logger;

        public PlaylistService(IPlaylistRepository playlistRepository, ITrackRepository trackRepository, IMapper mapper, ILogger<PlaylistService> logger)
        {
            _playlistRepository = playlistRepository;
            _trackRepository = trackRepository;
            _logger = logger;
            _mapper = mapper;
        }

         public async Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
            var track = await _trackRepository.GetTrackByIdAsync(trackId);

            if (playlist != null && track != null)
            {
                playlist.AddTrack(track);
                await _playlistRepository.UpdatePlaylistAsync(playlist); // Save changes to the playlist
            }
        }

        public async Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
            var track = await _trackRepository.GetTrackByIdAsync(trackId);

            if (playlist != null && track != null)
            {
                playlist.RemoveTrack(track);
                await _playlistRepository.UpdatePlaylistAsync(playlist); // Save changes to the playlist
            }
        }

        public async Task<PlaylistDTO> GetPlaylistByIdAsync(Guid playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
            if (playlist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistId} not found.");
            }
            return _mapper.Map<PlaylistDTO>(playlist);
        }

        public async Task<IEnumerable<PlaylistDTO>> GetPlaylistByUserIdAsync(Guid userId)
        {
            var playlists = await _playlistRepository.GetAllPlaylistsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<PlaylistDTO>>(playlists);
        }

        public async Task<Guid> CreatePlaylistAsync(PlaylistDTO playlistDTO)
        {
            if (playlistDTO == null)
            {
                throw new ArgumentNullException(nameof(playlistDTO), "Playlist DTO cannot be null.");
            }

            var playlist = _mapper.Map<Playlist>(playlistDTO);
            
            await _playlistRepository.AddPlaylistAsync(playlist);
            return playlist.Id;
        }

        public async Task<bool> UpdatePlaylistAsync(PlaylistDTO playlistDTO)
        {
            if (playlistDTO == null)
            {
                throw new ArgumentNullException(nameof(playlistDTO), "Playlist DTO cannot be null.");
            }

            var existingPlaylist = await _playlistRepository.GetPlaylistByIdAsync(playlistDTO.Id);
            if (existingPlaylist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistDTO.Id} not found.");
            }

            var playlist = _mapper.Map<Playlist>(playlistDTO);
            await _playlistRepository.UpdatePlaylistAsync(playlist);
            return true;
        }


        public async Task<bool> DeletePlaylistAsync(Guid playlistId)
        {
            var existingPlaylist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
            if (existingPlaylist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistId} not found.");
            }

            await _playlistRepository.DeletePlaylistAsync(playlistId);
            return true;
        }

        public async Task<IEnumerable<PlaylistDTO>> GetPlaylistsAllAsync()
        {
            var playlists = await _playlistRepository.GetAllPlaylistsAsync();
            foreach (var playlist in playlists)
            {
                if (playlist != null)
                {
                    _logger.LogInformation($"Playlist: {playlist.Title}, User: {playlist.User?.Email}");
                
                  if(playlist.PlaylistTracks != null && playlist.PlaylistTracks.Any())
                {
                    foreach (var playlistTrack in playlist.PlaylistTracks)
                    {
                         var track = playlistTrack.Track; // Access the Track
                        if (track != null) // Ensure the Track is not null
                        {
                            _logger.LogInformation($"Track: {track.Title}, Artist: {track.Artist}");
                        }
                    }
                }
                }
            }
            return _mapper.Map<IEnumerable<PlaylistDTO>>(playlists);
        }
    }
}
