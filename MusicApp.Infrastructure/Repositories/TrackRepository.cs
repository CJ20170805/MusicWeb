using Microsoft.EntityFrameworkCore;
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

        public TrackRepository(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Track> GetTrackByIdAsync(Guid trackId)
        {
            var track = await _context.Tracks.FindAsync(trackId);
            if(track == null)
            {
                throw new Exception("Track not found");
            }
            return track;
        }

        public async Task<IEnumerable<Track>> GetAllTracksAsync()
        {
            var tracks =  await _context.Tracks
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
            _context.Tracks.Update(track);
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
