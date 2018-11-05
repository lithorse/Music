using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Music.Model;
using Music.UI.Data;
using Music.UI.Data.Repositories;
using Music.UI.Event;
using Music.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace Music.UI.ViewModel
{
    class SongDetailViewModel : ViewModelBase, ISongDetailViewModel
    {
        private ISongRepository _songRepository;
        private IEventAggregator _eventAggregator;
        private SongWrapper _song;
        private bool _hasChanges;

        public SongDetailViewModel(ISongRepository songRepository, IEventAggregator eventAggregator)
        {
            _songRepository = songRepository;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        

        public async Task LoadAsync(int? songId)
        {
            var song = songId.HasValue ? await _songRepository.GetByIdAsync(songId.Value) : CreateNewSong();

            Song = new SongWrapper(song);
            Song.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _songRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Song.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Song.Id == 0)
            {
                Song.Name = "";
            }
        }

        public SongWrapper Song
        {
            get { return _song; }
            private set
            {
                _song = value;
                OnPropertyChanged();
            }
        }


        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        private async void OnSaveExecute()
        {
            await _songRepository.SaveAsync();
            HasChanges = _songRepository.HasChanges();
            _eventAggregator.GetEvent<AfterSongSavedEvent>().Publish(new AfterSongSavedEventArgs()
            {
                Id = Song.Id,
                DisplayMember = Song.Name
            });
        }

        private bool OnSaveCanExecute()
        {
            return Song != null && !Song.HasErrors && HasChanges;
        }

        private Song CreateNewSong()
        {
            var song = new Song();
            _songRepository.Add(song);
            return song;
        }

        private async void OnDeleteExecute()
        {
            var result = MessageBox.Show("Really delete?", "Question", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            _songRepository.Remove(Song.Model);
            await _songRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterSongDeletedEvent>().Publish(Song.Id);
        }
    }
}
