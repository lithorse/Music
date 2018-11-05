using System.Threading.Tasks;

namespace Music.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id);
        bool HasChanges { get; }
    }
}