using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music.Model;

namespace Music.UI.Data.Lookups
{
    public interface IAlbumLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetAlbumLookupAsync();
    }
}
