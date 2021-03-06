﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Music_App.Class
{
    public class DownloadSong
    {
        // Name, Artist, Path, URL,Stream, Image
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
            set { _artist = value; }
        }

        string _image;
        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }

        string _lyric;
        public string Lyric
        {
            get { return _lyric; }
            set { _lyric = value; }
        }

        public bool bool_on;

        string _stream;
        public string Stream
        {
            get { return _stream; }
            set
            {
                _stream = value;
            }
        }

        private int current_index;
        public int Current_Index
        {
            get { return current_index; }
            set { current_index = value; }
        }

        string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public DownloadSong()
        {
        }

        public DownloadSong(MySong song)
        {
            Path = song.Path;
            Name = song.Name;
            URL = song.URL;
            Artist = song.Artist.Name;
            Image = song.Image;
            Lyric = song.Lyric;
            bool_on = song.bool_on;
            Stream = song.Stream;
        }
    }
}
