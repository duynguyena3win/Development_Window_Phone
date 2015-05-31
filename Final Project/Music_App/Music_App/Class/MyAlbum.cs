using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_App.Class
{
    public class MyAlbum : INotifyPropertyChanged
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

        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        MyArtist artist;
        public MyArtist Artist
        {
            get { return artist; }
            set { artist = value; }
        }

        ObservableCollection<MySong> songs;
        public ObservableCollection<MySong> Songs
        {
            get { return songs; }
            set { songs = value; }
        }

        public MyAlbum()
        {
            songs = new ObservableCollection<MySong>();
            artist = new MyArtist();
        }
    }
}
