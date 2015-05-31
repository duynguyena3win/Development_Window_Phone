using Microsoft.Xna.Framework.Media;
using Music_App.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_App.Process
{
    public class MusicOffline
    {
        ObservableCollection<MyAlbum> Offline_Album;
        ObservableCollection<MyArtist> Offline_Artist;

        
        public ObservableCollection<MyAlbum> OffL_Album
        {
            get { return Offline_Album; }
            set { Offline_Album = value; }
        }
        public ObservableCollection<MyArtist> OffL_Artist
        {
            get { return Offline_Artist; }
            set { Offline_Artist = value; }
        }

        public SongCollection List_Current_Song;

        ObservableCollection<MySong> offL_Current_Songs;
        public ObservableCollection<MySong> OffL_Current_Songs
        {
            get { return offL_Current_Songs; }
            set { offL_Current_Songs = value; }
        }
        
        public void Update_LibrarySongs()
        {
            MediaLibrary MLibrary = new MediaLibrary();
            List_Current_Song = MLibrary.Songs;
            OffL_Current_Songs.Clear();
            foreach (Song so in MLibrary.Songs)
            {
                MySong temp = new MySong();
                temp.Name = so.Name;
                temp.Artist.Name = so.Artist.Name;
                temp.Album.Name = so.Album.Name;
                //temp.Genre = so.Genre.Name;
                OffL_Current_Songs.Add(temp);
            }
        }
        public void Update_LibraryArtists()
        {
            MediaLibrary MLibrary = new MediaLibrary();
            Offline_Artist.Clear();
            foreach (Artist ar in MLibrary.Artists)
            {
                MyArtist temp = new MyArtist();
                temp.Name = ar.Name;
                foreach(Song so in ar.Songs)
                {
                    MySong t1 = new MySong();
                    t1.Name = so.Name;
                    //t1.Genre = so.Genre.Name;
                    t1.Artist.Name = so.Artist.Name;
                    t1.Album.Name = so.Album.Name;
                    temp.Songs.Add(t1);
                }

                foreach (Album al in ar.Albums)
                {
                    MyAlbum t1 = new MyAlbum();
                    t1.Name = al.Name;
                    t1.Artist.Name = al.Artist.Name;
                    temp.Albums.Add(t1);
                }
                Offline_Artist.Add(temp);
            }
        }
        public void Update_LibraryAlbums()
        {
            MediaLibrary MLibrary = new MediaLibrary();
            
            Offline_Album.Clear();
            foreach (Album ar in MLibrary.Albums)
            {
                MyAlbum temp = new MyAlbum();
                temp.Name = ar.Name;
                foreach (Song so in ar.Songs)
                {
                    MySong t1 = new MySong();
                    t1.Name = so.Name;
                    //t1.Genre = so.Genre.Name;
                    t1.Artist.Name = so.Artist.Name;
                    t1.Album.Name = so.Album.Name;
                    temp.Songs.Add(t1);
                }

                temp.Artist.Name = ar.Artist.Name;
                Offline_Album.Add(temp);
            }
        }


        public MusicOffline()
        {
            Offline_Album = new ObservableCollection<MyAlbum>();
            Update_LibraryAlbums();

            Offline_Artist = new ObservableCollection<MyArtist>();
            Update_LibraryArtists();

            offL_Current_Songs = new ObservableCollection<MySong>();
            Update_LibrarySongs();

            MediaLibrary ML = new MediaLibrary();
            List_Current_Song = ML.Songs;
        }
        public void Load_Songs_Album(int index)
        {
            MediaLibrary ML = new MediaLibrary();
            List_Current_Song = ML.Albums[index].Songs;

            OffL_Current_Songs.Clear();
            foreach (Song so in ML.Albums[index].Songs)
            {
                MySong temp = new MySong();
                temp.Name = so.Name;
                temp.Artist.Name = so.Artist.Name;
                temp.Album.Name = so.Album.Name;
                //temp.Genre = so.Genre.Name;
                temp.Stream = null;
                OffL_Current_Songs.Add(temp);
            }
        }
        public void Load_Songs_Artist(int index)
        {
            MediaLibrary ML = new MediaLibrary();
            List_Current_Song = ML.Artists[index].Songs;

            OffL_Current_Songs.Clear();
            foreach (Song so in ML.Artists[index].Songs)
            {
                MySong temp = new MySong();
                temp.Name = so.Name;
                temp.Artist.Name = so.Artist.Name;
                temp.Album.Name = so.Album.Name;
                //temp.Genre = so.Genre.Name;
                temp.Stream = null;
                OffL_Current_Songs.Add(temp);
            }
        }

        public void Load_Songs()
        {
            MediaLibrary ML = new MediaLibrary();
            List_Current_Song = ML.Songs;

            OffL_Current_Songs.Clear();
            foreach (Song so in ML.Songs)
            {
                MySong temp = new MySong();
                temp.Name = so.Name;
                temp.Artist.Name = so.Artist.Name;
                temp.Album.Name = so.Album.Name;
                //temp.Genre = so.Genre.Name;
                temp.Stream = null;
                temp.Image = "/Assets/Picture/Music_Offline.png";
                OffL_Current_Songs.Add(temp);
            }
        }
    }
}
