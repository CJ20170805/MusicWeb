using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.DTOs;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace MusicApp.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly ITrackService _trackService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public PlaylistController(IPlaylistService playlistService, ITrackService trackService, IMapper mapper, UserManager<User> userManager)
        {
            _playlistService = playlistService;
            _trackService = trackService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("{playlistId}/add-track/{trackId}")]
        public async Task<IActionResult> AddTrackToPlaylist(Guid playlistId, Guid trackId)
        {
            // Fetch the playlist and track using services
            var playlistDTO = await _playlistService.GetPlaylistByIdAsync(playlistId);
            if (playlistDTO == null)
            {
                return NotFound("Playlist not found");
            }

            var trackDTO = await _trackService.GetTrackByIdAsync(trackId);
            if (trackDTO == null)
            {
                return NotFound("Track not found");
            }

            // Add the track to the playlist
            await _playlistService.AddTrackToPlaylistAsync(playlistId, trackId);
            return Ok("Add Succeed");
        }

        [HttpDelete("{playlistId}/remove-track/{trackId}")]
        public async Task<IActionResult> RemoveTrackFromPlaylist(Guid playlistId, Guid trackId)
        {
            // Fetch the playlist and track using services
            var playlistDTO = await _playlistService.GetPlaylistByIdAsync(playlistId);
            if (playlistDTO == null)
            {
                return NotFound("Playlist not found");
            }

            var trackDTO = await _trackService.GetTrackByIdAsync(trackId);
            if (trackDTO == null)
            {
                return NotFound("Track not found");
            }

            // Remove the track from the playlist
            await _playlistService.RemoveTrackFromPlaylistAsync(playlistId, trackId);

            return NoContent(); // 204 No Content on success
        }

       //Get: api/playlist/{userId}
       [HttpGet("user/{userId}")]
       public async Task<IActionResult> GetAllPlaylists(Guid userId)
       {
            var playlists = await _playlistService.GetPlaylistByUserIdAsync(userId);
            return Ok(playlists);
       }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylistById(Guid id)
        {
            var playlist = await _playlistService.GetPlaylistByIdAsync(id);
            return Ok(playlist);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            if(playlistDTO == null)
            {
                return BadRequest("Playlist object is null");
            }

            var userExists = await _userManager.FindByIdAsync(playlistDTO.UserId.ToString());
            if (userExists == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            var createdPlaylistId = await _playlistService.CreatePlaylistAsync(playlistDTO);
            return CreatedAtAction(nameof(GetPlaylistById), new { id = createdPlaylistId }, playlistDTO);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            if(playlistDTO == null)
            {
                return BadRequest("Playlist object is null");
            }
            var updated = await _playlistService.UpdatePlaylistAsync(playlistDTO);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            var deleted =  await _playlistService.DeletePlaylistAsync(id);
            if(!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
