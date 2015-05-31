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
    public class MyArtist : INotifyPropertyChanged
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

        ObservableCollection<MySong> songs = new ObservableCollection<MySong>();
        public ObservableCollection<MySong> Songs
        {
            get { return songs; }
            set { songs = value; }
        }

        ObservableCollection<MyAlbum> albums;

        public ObservableCollection<MyAlbum> Albums
        {
            get { return albums; }
            set { albums = value; }
        }
        

        public MyArtist()
        {
            albums = new ObservableCollection<MyAlbum>();
        }
    }
}
