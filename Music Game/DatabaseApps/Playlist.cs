using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DatabaseApps
{
    public class Playlist : INotifyPropertyChanged
    {
        string name_Playlist;

        public string Name_Playlist
        {
            get { return name_Playlist; }
            set { name_Playlist = value; }
        }

        string name_Artist;

        public string Name_Artist
        {
            get { return name_Artist; }
            set { name_Artist = value; }
        }

        string image_Playlist;

        public string Image_Playlist
        {
            get { return image_Playlist; }
            set { image_Playlist = value; }
        }

        int top;

        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        string url;

        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private List<Song> song_Playlist;

        public List<Song> Song_Playlist
        {
            get { return song_Playlist; }
            set { song_Playlist = value; }
        }
        public Playlist()
        {
            song_Playlist = new List<Song>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
