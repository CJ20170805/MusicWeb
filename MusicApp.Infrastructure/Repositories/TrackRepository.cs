using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;
using MusicApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApp.Infrastructure.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly MusicDbContext _context;
        private readonly ILogger<TrackRepository> _logger;

        public TrackRepository(MusicDbContext context, ILogger<TrackRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Track> GetTrackByIdAsync(Guid trackId)
        {
            var track = await _context.Tracks
             .Include(t => t.AudioFile)
                .Include(t => t.CoverImage)
                .FirstOrDefaultAsync(t => t.Id == trackId);

            if(track == null)
            {
                throw new Exception("Track not found");
            }
            return track;
        }

        public async Task<IEnumerable<Track>> GetAllTracksAsync()
        {
            var tracks =  await _context.Tracks
                .Include(t => t.AudioFile)
                .Include(t => t.CoverImage)
                .ToListAsync();
                
            if(tracks == null)
            {
                throw new Exception("No tracks found");
            }
            return tracks;
        }

        public async Task AddTrackAsync(Track track)
        {
            await _context.Tracks.AddAsync(track);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTrackAsync(Track track)
        {
             _logger.LogInformation("Attempting to update track with ID: {TrackId}", track.Id);
            // _context.Tracks.Update(track);
            // update track
            // _context.Tracks.Attach(track);
            // _context.Entry(track).State = EntityState.Modified;
            var existTrack = await _context.Tracks.FindAsync(track.Id);
            if (existTrack == null)
            {
                 _logger.LogWarning("Track with ID {TrackId} not found.", track.Id);
                throw new Exception("Track not found");
            }
            // _context.Tracks.Attach(existTrack);
            _context.Entry(existTrack).CurrentValues.SetValues(track);
            await _context.SaveChangesAsync();
          
        }

        public async Task DeleteTrackAsync(Guid trackId)
        {
            var track = await GetTrackByIdAsync(trackId);
            if (track != null)
            {
                _context.Tracks.Remove(track);
                await _context.SaveChangesAsync();
            }
        }
    }
}
