using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HtmlAgilityPack;
using WAO_Player.Class;
using System.ComponentModel;

namespace WAO_Player.WindowChild
{
    public partial class Online : PhoneApplicationPage, INotifyPropertyChanged
    {
        ListBox_Song LBS;
        private WebClient WebClient;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public Online()
        {
            this.DataContext = this;
            InitializeComponent();
            WebClient = new WebClient();
            WebClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            LBS = new ListBox_Song();
        }

        void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            LBS.Items = GetListSong(e.Result);
            List_Search_Song.ItemsSource = LBS.Items;
        }

        private List<Song_W> GetListSong(string Html)
        {
            List<Song_W> listSongs = new List<Song_W>();
            HtmlDocument docPage = new HtmlDocument();
            docPage.LoadHtml(Html);
            HtmlNode nodeListSong = docPage.DocumentNode.SelectSingleNode("//ul[@class='list-song']");
            if (nodeListSong == null)
                return null;
            HtmlNodeCollection nodeSongs = nodeListSong.SelectNodes("li");
            if (nodeSongs == null)
                return null;
            foreach (HtmlNode nodeSong in nodeSongs)
            {
                if (listSongs.Count == 10)
                    return listSongs;
                Song_W song = new Song_W();

                HtmlNode songName = nodeSong.SelectSingleNode("div[@class='song-name']");
                HtmlNodeCollection collectionNodesAInSongName = songName.SelectNodes(".//a");
                string quality = collectionNodesAInSongName[0].GetAttributeValue("title", "");
                quality = quality.Substring(quality.Count() - 5, 3);

                song.Quality = quality + " kbit/s";
                if (song.Quality.Contains("ici"))
                    song.Quality = "128 kbit/s";

                song.Name = collectionNodesAInSongName[1].GetAttributeValue("title", "");
                song.URL = collectionNodesAInSongName[1].GetAttributeValue("href", "");

                HtmlNode singers = nodeSong.SelectSingleNode("div[@class='singer']");
                foreach (HtmlNode singer in singers.ChildNodes)
                {
                    song.Artist += " " + singer.InnerText;
                }
                
                song.Album = "Nhac Cua Tui";
                listSongs.Add(song);
            }
            return listSongs;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string s = Static_String.Search_Songs(Text_Search.Text, 1);
            WebClient.DownloadStringAsync(new System.Uri(s));
        }

        private void Text_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text_Search.Text != String.Empty)
                Button_Search.IsEnabled = true;
            else
                Button_Search.IsEnabled = false;
        }
    }
}