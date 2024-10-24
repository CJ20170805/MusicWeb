using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.DTOs;
using MusicApp.Application.Interfaces;

namespace MusicApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService _trackService;

        public TrackController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        [HttpGet("trackId")]
        public async Task<IActionResult> GetTrackById(Guid trackId)
        {
            var track = await _trackService.GetTrackByIdAsync(trackId);
            if (track == null)
            {
                return NotFound();
            }
            return Ok(track);
        }

        //GetTracksAllAsync
        [HttpGet("All")]
        public async Task<IActionResult> GetTracksAll()
        {
            var tracks = await _trackService.GetTracksAllAsync();
            return Ok(tracks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrack([FromBody] TrackDTO trackDTO)
        {
            if(trackDTO == null)
            {
                return BadRequest();
            }
            var trackId = await _trackService.CreateTrackAsync(trackDTO);
            return CreatedAtAction(nameof(GetTrackById), new { trackId }, trackDTO);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTrack([FromBody] TrackDTO trackDTO)
        {
            if (trackDTO == null)
            {
                return BadRequest();
            }
            var result = await _trackService.UpdateTrackAsync(trackDTO);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("trackId")]
        public async Task<IActionResult> DeleteTrack(Guid trackId)
        {
            var result = await _trackService.DeleteTrackAsync(trackId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
