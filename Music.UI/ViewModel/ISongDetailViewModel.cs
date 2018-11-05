using System.Threading.Tasks;

namespace Music.UI.ViewModel
{
    public interface ISongDetailViewModel
    {
        Task LoadAsync(int? songId);
        bool HasChanges { get; }
    }
}