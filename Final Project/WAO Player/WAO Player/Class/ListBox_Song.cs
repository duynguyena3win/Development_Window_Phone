using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAO_Player.Class
{
    public class ListBox_Song : INotifyPropertyChanged
    {
        private List<Song_W> items;

        public List<Song_W> Items
        {
            get { return items; }
            set { items = value; }
        }

        public ListBox_Song()
        {
            Items = new List<Song_W>();
        }
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            Items.Add(new Song_W() { Name = "My love", Artist = "Westlife", Genre = "POP" });
            Items.Add(new Song_W() { Name = "The one that got away", Artist = "JNevermid", Genre = "POP" });
            Items.Add(new Song_W() { Name = "Day by day", Artist = "T-ara", Genre = "POP" });
            Items.Add(new Song_W() { Name = "Hello", Artist = "Jame", Genre = "POP" });

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
