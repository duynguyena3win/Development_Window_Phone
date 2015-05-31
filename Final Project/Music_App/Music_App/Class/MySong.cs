using Microsoft.Phone.BackgroundAudio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_App.Class
{
    public class MySong : INotifyPropertyChanged
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
        Song _offlineSong;

        public Song OfflineSong
        {
            get { return _offlineSong; }
            set { _offlineSong = value; }
        }
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        string _url;
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        MyArtist _artist;
        public MyArtist Artist
        {
            get { return _artist; }
            set { _artist = value; NotifyPropertyChanged("Artist"); }
        }

         AudioTrack _offline_Track;

        public AudioTrack Offline_Track
        {
            get { return _offline_Track; }
            set { _offline_Track = value; }
        }

        MyAlbum _album;
        public MyAlbum Album
        {
            get { return _album; }
            set { _album = value; NotifyPropertyChanged("Album"); }
        }

        string _image;
        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyPropertyChanged("Image");
            }
        }
        
        string _stream;
        public string Stream
        {
            get { return _stream; }
            set
            {
                try
                {
                    _stream = value;
                    if (Image.Length > 1 && Offline_Track == null)
                        Offline_Track = new AudioTrack(new Uri(_stream, UriKind.Absolute), Name, Artist.Name, Album.Name, null, Image, EnabledPlayerControls.All);
                    bool_on = true;
                }
                catch { }
            }
        }

        string _lyric;

        public string Lyric
        {
            get { return _lyric; }
            set { _lyric = value; NotifyPropertyChanged("Lyric"); }
        }

        public bool bool_on;

        public MySong()
        {
            _stream = null;
            _url = null;
            _image = null;
            bool_on = false;
            _album = new MyAlbum();
            _artist = new MyArtist();
        }

        public MySong(Song so)
        {
            bool_on = false;
            _stream = null;
            Name = so.Name;
            this.Artist = new MyArtist();
            Artist.Name = so.Artist.Name;
            this.Album = new MyAlbum();
            Album.Name = so.Album.Name;
            Lyric = null;
            //Genre = so.Genre.Name;
            OfflineSong = so;
            Image = "/Assets/Picture/Music_Offline.png";
        }

        public MySong(MySong so)
        {
            Name = so.Name;
            this.Artist = new MyArtist();
            Artist.Name = so.Artist.Name;
            this.Album = new MyAlbum();
            Album.Name = so.Album.Name;
            //Genre = so.Genre;
            URL = so.URL;
            Lyric = so.Lyric;
            Image = so.Image;
            Stream = so.Stream;
        }

        public MySong(HistorySong so)
        {
            Name = so.Name;
            this.Artist = new MyArtist();
            Artist.Name = so.Artist;
            this.Album = new MyAlbum();
            URL = so.URL;
            bool_on = so.bool_on;
            Lyric = so.Lyric;
            Image = so.Image;
            Stream = so.Stream;
        }

        public void CopyTo(MySong so)
        {
            _name = so.Name;
            Artist.Name = so.Artist.Name;
            this.Album = new MyAlbum();
            Album.Name = so.Album.Name;
            //_genre = so.Genre;
            _image = so.Image;
            Lyric = so.Lyric;
            Stream = so.Stream;
        }
    }
}
