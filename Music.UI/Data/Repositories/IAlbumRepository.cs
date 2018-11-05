using System.Collections.Generic;
using System.Threading.Tasks;
using Music.Model;

namespace Music.UI.Data.Repositories
{
    public interface IAlbumRepository : IGenericRepository<Album>
    {
        Task<List<Song>> GetAllSongsAsync();
    }
}