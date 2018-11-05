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
        private IAlbumLookupDataService _albumLookupDataService;
        private IEventAggregator _eventAggregator;

        public NavigationViewModel(ISongLookupDataService songLookupService, IAlbumLookupDataService albumLookupDataService, IEventAggregator eventAggregator)
        {
            _songLookupService = songLookupService;
            _albumLookupDataService = albumLookupDataService;
            _eventAggregator = eventAggregator;
            Songs = new ObservableCollection<NavigationItemViewModel>();
            Albums = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            var lookup = await _songLookupService.GetSongLookupAsync();
            Songs.Clear();
            foreach (var item in lookup)
            {
                Songs.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(SongDetailViewModel), _eventAggregator));
            }
            lookup = await _albumLookupDataService.GetAlbumLookupAsync();
            Albums.Clear();
            foreach (var item in lookup)
            {
                Albums.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(AlbumDetailViewModel), _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Songs { get; }
        public ObservableCollection<NavigationItemViewModel> Albums { get; }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(SongDetailViewModel):
                    AfterDetailDeleted(Songs, args);
                    break;
                case nameof(AlbumDetailViewModel):
                    AfterDetailDeleted(Albums, args);
                    break;
            }
        }
        
        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(s => s.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(SongDetailViewModel):
                    AfterDetailSaved(Songs, args);
                    break;
                case nameof(AlbumDetailViewModel):
                    AfterDetailSaved(Albums, args);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(s => s.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, args.ViewModelName, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }
    }
}
