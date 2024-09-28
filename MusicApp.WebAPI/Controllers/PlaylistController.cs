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
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public PlaylistController(IPlaylistService playlistService, IMapper mapper, UserManager<User> userManager)
        {
            _playlistService = playlistService;
            _mapper = mapper;
            _userManager = userManager;
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
