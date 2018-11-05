using Music.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.UI.Wrapper
{
    public class AlbumWrapper : ModelWrapper<Album>
    {
        public AlbumWrapper(Album model) : base(model)
        {
        }

        public int Id
        {
            get { return Model.Id; }
        }

        public string Title
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
