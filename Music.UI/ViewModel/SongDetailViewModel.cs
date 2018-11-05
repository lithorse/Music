using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Music.Model;
using Music.UI.Data;
using Music.UI.Data.Repositories;
using Music.UI.Event;
using Music.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace Music.UI.ViewModel
{
    class SongDetailViewModel : DetailViewModelBase, ISongDetailViewModel
    {
        private ISongRepository _songRepository;
        private SongWrapper _song;
        private bool _hasChanges;

        public SongDetailViewModel(ISongRepository songRepository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _songRepository = songRepository;
        }

        

        public override async Task LoadAsync(int? songId)
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

        protected override async void OnSaveExecute()
        {
            await _songRepository.SaveAsync();
            HasChanges = _songRepository.HasChanges();
            RaiseDetailSavedEvent(Song.Id, Song.Name);
        }

        protected override bool OnSaveCanExecute()
        {
            return Song != null && !Song.HasErrors && HasChanges;
        }

        private Song CreateNewSong()
        {
            var song = new Song();
            _songRepository.Add(song);
            return song;
        }

        protected override async void OnDeleteExecute()
        {
            var metroWindow = (MetroWindow) App.Current.MainWindow;
            var result =
                await metroWindow.ShowMessageAsync("Delete", "Really delete?", MessageDialogStyle.AffirmativeAndNegative);
            if (result != MessageDialogResult.Affirmative)
            {
                return;
            }
            _songRepository.Remove(Song.Model);
            await _songRepository.SaveAsync();
            RaiseDetailDeletedEvent(Song.Id);
        }
    }
}
