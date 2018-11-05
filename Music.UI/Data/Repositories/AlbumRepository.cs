using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music.DataAccess;
using Music.Model;

namespace Music.UI.Data.Repositories
{
    public class AlbumRepository : GenericRepository<Album, MusicDbContext>, IAlbumRepository
    {
        public AlbumRepository(MusicDbContext context) : base(context)
        {
        }

        public async override Task<Album> GetByIdAsync(int id)
        {
            return await Context.Albums.Include(a => a.Songs).SingleAsync(a => a.Id == id);
        }

        public async Task<List<Song>> GetAllSongsAsync()
        {
            return await Context.Set<Song>().ToListAsync();
        }
    }
}
