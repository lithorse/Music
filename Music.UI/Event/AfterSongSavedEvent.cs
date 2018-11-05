using Prism.Events;

namespace Music.UI.Event
{
    public class AfterSongSavedEvent:PubSubEvent<AfterSongSavedEventArgs>
    {
    }

    public class AfterSongSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
