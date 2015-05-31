using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAO_Player.Class
{
    public class Song_W : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
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

        string _url;

        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }
        string _artist;

        public string Artist
        {
            get { return _artist; }
            set { _artist = value; NotifyPropertyChanged("Artist"); }
        }

        string _genre;

        public string Genre
        {
            get { return _genre; }
            set { _genre = value; NotifyPropertyChanged("Genre"); }
        }
        string _lyric;

        public string Lyric
        {
            get { return _lyric; }
            set { _lyric = value; }
        }
        string _quality;

        public string Quality
        {
            get { return _quality; }
            set { _quality = value; NotifyPropertyChanged("Quality"); }
        }
        string _album;

        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }

        string _image;

        public string Image
        {
            get { return _image; }
            set { _image = value; NotifyPropertyChanged("Image"); }
        }
        string _stream;

        public string Stream
        {
            get { return _stream; }
            set { _stream = value; }
        }
        public Song_W()
        {
            Stream = null;
        }
    }
}
