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
    public class SongRepository : GenericRepository<Song, MusicDbContext>, ISongRepository
    {

        public SongRepository(MusicDbContext context) : base (context)
        {
        }
        
        public override async Task<Song> GetByIdAsync(int songId)
        {
            return await Context.Songs.SingleAsync(f => f.Id == songId);
        }
    }
}
