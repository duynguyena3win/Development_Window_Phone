using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatabaseApps
{
    public class Song : INotifyPropertyChanged
    {
        private bool ishave;

        public bool isHave
        {
            get { return ishave; }
            set { ishave = value; }
        }

        private string name_Song;

        public string Name_Song
        {
            get { return name_Song; }
            set
            {
                if (value == null)
                    name_Song = "unknow";
                else
                    name_Song = value;
            }
        }
        private string name_Artist;

        public string Name_Artist
        {
            get { return name_Artist; }
            set
            {
                if (value == null)
                    name_Artist = "unknow";
                else
                    name_Artist = value;
            }
        }
        private string name_Album;

        public string Name_Album
        {
            get { return name_Album; }
            set
            {
                if (value == null)
                    name_Album = "unknow";
                else
                    name_Album = value;
            }
        }
        private string genre;

        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }
        private TimeSpan lenght;

        public TimeSpan Lenght
        {
            get { return lenght; }
            set { lenght = value; }
        }
        private string uRL;

        public string URL
        {
            get { return uRL; }
            set { uRL = value; }
        }
        private string image_Song;

        public string Image_Song
        {
            get { return image_Song; }
            set { image_Song = value; }
        }
        private string lyric_Song;

        public string Lyric_Song
        {
            get { return lyric_Song; }
            set { lyric_Song = value; }
        }
        // Music Online
        private int top;

        public int Top
        {
            get { return top; }
            set { top = value; }
        }
        private string quality;

        public bool Online = false;

        public string Quality
        {
            get { return quality; }
            set
            {
                if (value == null)
                    quality = "unknow";
                else
                    quality = value;
            }
        }
        public override string ToString()
        {
            return String.Format("{0}", Name_Song);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Song()
        {
            Lenght = new TimeSpan();
            isHave = true;
            Top = -1;
        }
        public Song(XElement t)
        {
            Name_Song = t.Attribute("Name_Song").Value;
            Name_Artist = t.Attribute("Name_Artist").Value;
            Name_Album = t.Attribute("Name_Album").Value;
            Genre = t.Attribute("Genre").Value;
            URL = t.Attribute("URL").Value;
            Lenght = TimeSpan.Parse(t.Attribute("Lenght").Value);
            Top = -1;
            Image_Song = null;
            isHave = true;
        }

        public Song(string path)
        {
            try
            {
                TagLib.File tagFile = TagLib.File.Create(path);

                if (tagFile.Tag.FirstPerformer != null)
                    Name_Artist = tagFile.Tag.FirstPerformer;
                else
                    Name_Artist = "unknow";

                if (tagFile.Tag.Album != null)
                    Name_Album = tagFile.Tag.Album;
                else
                    Name_Album = "unknow";

                if (tagFile.Tag.FirstGenre != null)
                    Genre = tagFile.Tag.FirstGenre;
                else
                    Genre = "unknow";

                if (tagFile.Tag.Title != null)
                    Name_Song = tagFile.Tag.Title;
                else
                    Name_Song = "unknow";

                Lenght = tagFile.Properties.Duration;
                Lyric_Song = tagFile.Tag.Lyrics;
                Image_Song = null;
                URL = path;
                isHave = true;
            }
            catch
            {
                isHave = false;
            }
        }

        public void Edit_Song(string name, string aritist, string album, string genre, TimeSpan lenght, string url)
        {
            if (name != null)
                Name_Song = name;

            if (aritist != null)
                Name_Artist = aritist;

            if (album != null)
                Name_Album = album;

            if (genre != null)
                Genre = genre;

            if (lenght != null)
                Lenght = lenght;

            if (url != null)
                URL = url;
        }
    }
}
