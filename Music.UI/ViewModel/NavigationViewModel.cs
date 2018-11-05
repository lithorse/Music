using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music.Model;
using Music.UI.Data;
using Music.UI.Data.Lookups;
using Music.UI.Event;
using Prism.Events;

namespace Music.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private ISongLookupDataService _songLookupService;
        private IEventAggregator _eventAggregator;

        public NavigationViewModel(ISongLookupDataService songLookupService, IEventAggregator eventAggregator)
        {
            _songLookupService = songLookupService;
            _eventAggregator = eventAggregator;
            Songs = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterSongSavedEvent>().Subscribe(AfterSongSaved);
            _eventAggregator.GetEvent<AfterSongDeletedEvent>().Subscribe(AfterSongDeleted);
        }

        public async Task LoadAsync()
        {
            var lookup = await _songLookupService.GetSongLookupAsync();
            Songs.Clear();
            foreach (var item in lookup)
            {
                Songs.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Songs { get; }

        private void AfterSongDeleted(int songId)
        {
            var song = Songs.SingleOrDefault(s => s.Id == songId);
            if (song != null)
            {
                Songs.Remove(song);
            }
        }

        private void AfterSongSaved(AfterSongSavedEventArgs obj)
        {
            var lookupItem = Songs.SingleOrDefault(s => s.Id == obj.Id);
            if (lookupItem == null)
            {
                Songs.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }
    }
}
