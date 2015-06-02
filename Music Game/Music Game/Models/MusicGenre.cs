using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Game.Models
{
    public class MusicGenre : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        string source;

        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                NotifyPropertyChanged("Source");
            }
        }

        string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public MusicGenre()
        {
            Source = Title = null;
        }

        public MusicGenre(string title, string source)
        {
            Title = title;
            Source = source;
        }
    }
}
