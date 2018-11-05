using System.Collections.Generic;
using System.Threading.Tasks;
using Music.Model;

namespace Music.UI.Data.Lookups
{
    public interface ISongLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetSongLookupAsync();
    }
}