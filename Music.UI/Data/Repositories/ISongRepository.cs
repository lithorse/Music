using System.Collections.Generic;
using System.Threading.Tasks;
using Music.Model;

namespace Music.UI.Data.Repositories
{
    public interface ISongRepository
    {
        Task<Song> GetByIdAsync(int songId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Song song);
        void Remove(Song model);
    }
}