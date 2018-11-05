using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Music.Model;
using Music.UI.Data.Repositories;
using Music.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace Music.UI.ViewModel
{
    public class AlbumDetailViewModel : DetailViewModelBase, IAlbumDetailViewModel
    {
        private IAlbumRepository _albumRepository;
        private AlbumWrapper _album;
        private Song _selectedAvailableSong;
        private Song _selectedAddedSong;
        private List<Song> _allSongs;

        public AlbumDetailViewModel(IEventAggregator eventAggregator, IAlbumRepository albumRepository) : base(
            eventAggregator)
        {
            _albumRepository = albumRepository;

            AddedSongs = new ObservableCollection<Song>();
            AvailableSongs = new ObservableCollection<Song>();
            AddSongCommand = new DelegateCommand(OnAddSongExecute, OnAddSongCanExecute);
            RemoveSongCommand = new DelegateCommand(OnRemoveSongExecute, OnRemoveSongCanExecute);
        }

        public AlbumWrapper Album
        {
            get { return _album; }
            private set
            {
                _album = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddSongCommand { get; }

        public ICommand RemoveSongCommand { get; }

        public ObservableCollection<Song> AddedSongs { get; }

        public ObservableCollection<Song> AvailableSongs { get; }

        public Song SelectedAvailableSong
        {
            get { return _selectedAvailableSong; }
            set
            {
                _selectedAvailableSong = value;
                OnPropertyChanged();
                ((DelegateCommand)AddSongCommand).RaiseCanExecuteChanged();
            }
        }

        public Song SelectedAddedSong
        {
            get { return _selectedAddedSong; }
            set
            {
                _selectedAddedSong = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveSongCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? albumId)
        {
            var album = albumId.HasValue ? await _albumRepository.GetByIdAsync(albumId.Value) : CreateNewAlbum();
            InitializeAlbum(album);

            _allSongs = await _albumRepository.GetAllSongsAsync();

            SetupPicklist();
        }

        protected override async void OnDeleteExecute()
        {
            var result = MessageBox.Show("Really delete?", "Question", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            _albumRepository.Remove(Album.Model);
            await _albumRepository.SaveAsync();
            RaiseDetailDeletedEvent(Album.Id);
        }

        protected override bool OnSaveCanExecute()
        {
            return Album != null && !Album.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _albumRepository.SaveAsync();
            HasChanges = _albumRepository.HasChanges();
            RaiseDetailSavedEvent(Album.Id, Album.Title);
        }

        private void SetupPicklist()
        {
            var albumSongIds = Album.Model.Songs.Select(s => s.Id).ToList();
            var addedSongs = _allSongs.Where(s => albumSongIds.Contains(s.Id)).OrderBy(s => s.Id);
            var availableSongs = _allSongs.Except(addedSongs).OrderBy(s => s.Id);

            AddedSongs.Clear();
            AvailableSongs.Clear();
            foreach (var addedSong in addedSongs)
            {
                AddedSongs.Add(addedSong);
            }
            foreach (var availableSong in availableSongs)
            {
                AvailableSongs.Add(availableSong);
            }
        }

        private Album CreateNewAlbum()
        {
            var album = new Album();
            _albumRepository.Add(album);
            return album;
        }

        private void InitializeAlbum(Album album)
        {
            Album = new AlbumWrapper(album);
            Album.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _albumRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Album.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Album.Id == 0)
            {
                Album.Title = "";
            }
        }

        private void OnRemoveSongExecute()
        {
            var songToRemove = SelectedAddedSong;

            Album.Model.Songs.Remove(songToRemove);
            AddedSongs.Remove(songToRemove);
            AvailableSongs.Add(songToRemove);
            HasChanges = _albumRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveSongCanExecute()
        {
            return SelectedAddedSong != null;
        }

        private bool OnAddSongCanExecute()
        {
            return SelectedAvailableSong != null;
        }

        private void OnAddSongExecute()
        {
            var songToAdd = SelectedAvailableSong;

            Album.Model.Songs.Add(songToAdd);
            AddedSongs.Add(songToAdd);
            AvailableSongs.Remove(songToAdd);
            HasChanges = _albumRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }
}
