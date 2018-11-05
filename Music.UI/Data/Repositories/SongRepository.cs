using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Music.Model;
using Music.DataAccess;

namespace Music.UI.Data.Repositories
{
    public class SongRepository : ISongRepository
    {
        private MusicDbContext _context;

        public SongRepository(MusicDbContext context)
        {
            _context = context;
        }

        public void Add(Song song)
        {
            _context.Songs.Add(song);
        }

        public async Task<Song> GetByIdAsync(int songId)
        {
            return await _context.Songs.SingleAsync(f => f.Id == songId);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Remove(Song model)
        {
            _context.Songs.Remove(model);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
