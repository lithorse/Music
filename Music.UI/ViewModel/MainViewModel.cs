using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Music.Model;
using Music.UI.Data;
using Music.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace Music.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<ISongDetailViewModel> _songDetailViewModelCreator;
        private ISongDetailViewModel _songDetailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel, Func<ISongDetailViewModel> songDetailViewModelCreator, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _songDetailViewModelCreator = songDetailViewModelCreator;
            _eventAggregator.GetEvent<OpenSongDetailViewEvent>().Subscribe(OnOpenSongDetailView);
            _eventAggregator.GetEvent<AfterSongDeletedEvent>().Subscribe(AfterSongDeleted);

            CreateNewSongCommand = new DelegateCommand(OnCreateNewSongExecute);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewSongCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public ISongDetailViewModel SongDetailViewModel
        {
            get { return _songDetailViewModel; }
            private set
            {
                _songDetailViewModel = value; 
                OnPropertyChanged();
            }
        }

        private async void OnOpenSongDetailView(int? songId)
        {
            if (SongDetailViewModel != null && SongDetailViewModel.HasChanges)
            {
                var result = MessageBox.Show("You've made changes. Navigate away?", "Question", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            SongDetailViewModel = _songDetailViewModelCreator();
            await SongDetailViewModel.LoadAsync(songId);
        }

        private void OnCreateNewSongExecute()
        {
            OnOpenSongDetailView(null);
        }

        private void AfterSongDeleted(int songId)
        {
            SongDetailViewModel = null;
        }
    }
}
