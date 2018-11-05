using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Music.Model;

namespace Music.UI.Wrapper
{
    public class SongWrapper : ModelWrapper<Song>
    {
        public SongWrapper(Song model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    if (string.Equals(Name, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Robots can't be songs";
                    }
                    break;
            }
        }
        }
}
