using Microsoft.EntityFrameworkCore;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;
using MusicApp.Infrastructure.Data;

namespace MusicApp.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly MusicDbContext _context;

        public PlaylistRepository(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Playlist> GetPlaylistByIdAsync(Guid playlistId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null)
            {
                throw new Exception("Playlist not found");
            }
            return playlist;
        }

        public async Task<IEnumerable<Playlist>> GetAllPlaylistsByUserIdAsync(Guid userId)
        {
            return await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Playlist>> GetAllPlaylistsAsync()
        {
            return await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task AddPlaylistAsync(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePlaylistAsync(Playlist playlist)
        {
            var existingPlaylist = await _context.Playlists.FindAsync(playlist.Id);
            foreach(var p in _context.Playlists)
            {
                // print playlist Id and  playlist.id
                System.Console.WriteLine("Playlist:::::" + p.Title + "== " + p?.Id + "===" + playlist.Id);
            }
            System.Console.WriteLine("PPPPP" + existingPlaylist?.Title);
            if (existingPlaylist == null)
            {
                throw new Exception("Playlist not found");
            }
            _context.Entry(existingPlaylist).CurrentValues.SetValues(playlist);
            // _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlaylistAsync(Guid playlistId)
        {
            var playlist = await GetPlaylistByIdAsync(playlistId);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
        }
    }
}