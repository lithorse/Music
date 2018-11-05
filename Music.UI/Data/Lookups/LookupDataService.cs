using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music.DataAccess;
using Music.Model;

namespace Music.UI.Data.Lookups
{
    public class LookupDataService : ISongLookupDataService
    {
        private Func<MusicDbContext> _contextCreator;

        public LookupDataService(Func<MusicDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetSongLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Songs.AsNoTracking().Select(s => new LookupItem()
                {
                    Id = s.Id,
                    DisplayMember = s.Name
                }).ToListAsync();
            }
        }
    }
}
