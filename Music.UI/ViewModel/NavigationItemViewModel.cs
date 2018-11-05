using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Music.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace Music.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private IEventAggregator _eventAggregator;

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            OpenSongDetailViewCommand = new DelegateCommand(OnOpenSongDetailView);
            _eventAggregator = eventAggregator;
        }

        public int Id { get; }

        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenSongDetailViewCommand { get; }


        private void OnOpenSongDetailView()
        {
            _eventAggregator.GetEvent<OpenSongDetailViewEvent>().Publish(Id);
        }
    }
}
