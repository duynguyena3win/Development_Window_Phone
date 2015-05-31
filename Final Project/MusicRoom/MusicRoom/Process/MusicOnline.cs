using HtmlAgilityPack;
using Microsoft.Phone.BackgroundAudio;
using Music_App.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Music_App.Process
{
    public class MusicOnline : INotifyPropertyChanged
    {
        //  delegate and event
        public delegate void FindingCompleted();
        public event FindingCompleted LoadingListCompleted;

        public delegate void LoadingCompleted(MySong Output);
        public event LoadingCompleted LoadingSongCompleted;
        public event LoadingCompleted DownloadCompleted;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        short Genre;
        // Static Variable
        public const string URLSearch = "http://www.nhaccuatui.com/tim-kiem/bai-hat?q=";
        public const string URLNewSongs = "http://www.nhaccuatui.com/bai-hat/bai-hat-moi.";
        public const string URLSongPage = "http://www.nhaccuatui.com/bai-hat/";
        private static string[] KeyTop20 = {"top-20.nhac-viet.html",
                                                 "top-20.au-my.html",
                                                 "top-20.nhac-han.html"};
        // List Song Online
        ObservableCollection<MySong> onL_Current_Songs;
        public ObservableCollection<MySong> OnL_Current_Songs
        {
            get { return onL_Current_Songs; }
            set { onL_Current_Songs = value; NotifyPropertyChanged("OnL_Current_Songs"); }
        }

        // Variable
        int current_selected;
        public int Current_Selected
        {
            get { return current_selected; }
            set { current_selected = value; }
        }

        public bool State = true;
        int i_Search = -1;
        // Web Variable
        WebClient WClient;
        WebClient WClient_MoreInfo;
        WebClient WClient_ArtistInfo;
        WebClient WClient_Download;
        //Constructer
        public MusicOnline()
        {
            try
            {
                Init_WebClient();
                OnL_Current_Songs = new ObservableCollection<MySong>();
            }
            catch { }
        }

        void Init_WebClient()
        {
            try
            {
                WClient = new WebClient();
                WClient.DownloadStringCompleted += WClient_DownloadStringCompleted;

                WClient_MoreInfo = new WebClient();
                WClient_MoreInfo.DownloadStringCompleted += WClient_MoreInfo_DownloadStringCompleted;

                WClient_ArtistInfo = new WebClient();
                WClient_ArtistInfo.DownloadStringCompleted += WClient_ArtistInfo_DownloadStringCompleted;

                WClient_Download = new WebClient();
                WClient_Download.OpenReadCompleted += WClient_Download_OpenReadCompleted;
            }
            catch { }
        }

        void WClient_Download_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e1)
        {
            if (e1.Error == null)
            {
                try
                {
                    string filename = Song_Download.Stream.Substring(Song_Download.Stream.LastIndexOf("/") + 1).Trim();
                    bool isSpaceAvailable = IsSpaceIsAvailable(e1.Result.Length);

                    if (isSpaceAvailable)
                    {
                        // Save mp3 to Isolated Storage
                        try
                        {
                            using (var isfs = new IsolatedStorageFileStream(filename,
                                                FileMode.CreateNew,
                                                IsolatedStorageFile.GetUserStoreForApplication()))
                            {
                                long fileLen = e1.Result.Length;
                                byte[] b = new byte[fileLen];
                                e1.Result.Read(b, 0, b.Length);
                                isfs.Write(b, 0, b.Length);
                                isfs.Flush();
                                Song_Download.Path = isfs.Name;
                            }
                        }
                        catch(Exception ex) { }
                        Song_Download.Offline_Track = new AudioTrack(new Uri(Song_Download.Path, UriKind.Relative), Song_Download.Name,
                            Song_Download.Artist.Name, Song_Download.Album.Name, null, Song_Download.Image, EnabledPlayerControls.All);
                        Song_Download.bool_on = true;
                        DownloadCompleted(Song_Download);
                    }
                    else
                    {
                        MessageBox.Show("Not enough to save space available to download mp3.");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(e1.Error.Message);
            }
        }
        MySong Song_Download = new MySong();
        public void Download(MySong temp)
        {
            Song_Download = temp;
            WClient_Download.OpenReadAsync(new Uri(temp.Stream));
        }

        private bool IsSpaceIsAvailable(long spaceReq)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {

                long spaceAvail = store.AvailableFreeSpace;
                if (spaceReq > spaceAvail)
                {
                    return false;
                }
                return true;
            }
        }

        void WClient_ArtistInfo_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                OnL_Current_Songs[Current_Selected].Image = GetImageInfo(e.Result);
                MySong temp = new MySong(OnL_Current_Songs[Current_Selected]);
                LoadingSongCompleted(temp);
            }
            catch { }
            finally
            {
                State = true;
            }
        }

        void WClient_MoreInfo_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                OnL_Current_Songs[Current_Selected] = GetMoreSongInfo(e.Result, OnL_Current_Songs[Current_Selected]);
                WClient_ArtistInfo.DownloadStringAsync(new Uri(OnL_Current_Songs[Current_Selected].URL));
            }
            catch { }
        }
        
        void WClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (i_Search == 0)
                    OnL_Current_Songs = GetListSong(e.Result);
                else
                    if(i_Search == 1)
                        OnL_Current_Songs = GetListNewSong(e.Result);
                    else
                        OnL_Current_Songs = GetTop20Song(e.Result, Genre);
                LoadingListCompleted();
                State = true;
            }
            catch { }
        }

        public static string Search_Songs(string key, short pageNumber)
        {
            return URLSearch + key + "&page=" + pageNumber.ToString();
        }

        public ObservableCollection<MySong> GetListSong(string Html)
        {
            ObservableCollection<MySong> listSongs = new ObservableCollection<MySong>();
            try
            {
                HtmlDocument docPage = new HtmlDocument();
                docPage.LoadHtml(Html);
                HtmlNode nodeListSong = docPage.DocumentNode.SelectSingleNode("//div[@class='search_returns_frame']").ChildNodes[1];
                if (nodeListSong == null)
                    return null;
                HtmlNodeCollection nodeSongs = nodeListSong.SelectNodes("li");
                if (nodeSongs == null)
                    return null;
                foreach (HtmlNode nodeSong in nodeSongs)
                {
                    try
                    {
                        MySong song = new MySong();

                        HtmlNode songName = nodeSong.SelectSingleNode("div[@class='info_song']");
                        HtmlNodeCollection collectionNodesAInSongName = songName.SelectNodes(".//a");

                        song.Name = collectionNodesAInSongName[0].InnerText;
                        song.URL = collectionNodesAInSongName[0].GetAttributeValue("href", "");

                        for (int i = 1; i < collectionNodesAInSongName.Count; i++)
                        {
                            song.Artist.Name += collectionNodesAInSongName[i].InnerText;
                        }

                        song.Album.Name = "Nhạc Của Tui";
                        listSongs.Add(song);
                    }
                    catch { }
                }
                return listSongs;
            }
            catch { return listSongs; }
        }

        public string GetImageInfo(string Html)
        {
            try
            {
                string Image = "";
                HtmlDocument wap = new HtmlDocument();
                wap.LoadHtml(Html);

                try
                {
                    HtmlNode image = wap.DocumentNode.SelectSingleNode("//div[@class='box_singer_pri']").SelectSingleNode("//a[@class='avatar_user']").ChildNodes[0];
                    Image = image.GetAttributeValue("src", "");
                }
                catch
                {
                    Image = "/Assets/Picture/NCT_Image.png";
                }
                return Image;
            }
            catch { return null; }
        }

        public MySong GetMoreSongInfo(string Html, MySong Item)
        {
            try
            {
                string lyric = "";
                string linkStream;
                Item.Image = "";
                HtmlDocument wap = new HtmlDocument();
                wap.LoadHtml(Html);
                //Lấy lyrics
                try
                {
                    HtmlNode lyricNode = wap.DocumentNode.SelectSingleNode("//div[@class='lyric']");
                    foreach (HtmlNode node in lyricNode.ChildNodes)
                    {
                        lyric += WebUtility.HtmlDecode(node.InnerText);
                    }
                }
                catch { }
                Item.Lyric = lyric;
                //Lấy Stream
                try
                {
                    HtmlNode stream = wap.DocumentNode.SelectSingleNode("//div[@class='download']");
                    linkStream = stream.ChildNodes[1].ChildNodes[1].GetAttributeValue("href", "");

                }
                catch { linkStream = null; }

                Item.Stream = linkStream;
                return Item;
            }
            catch { return null; };
        }

        public void Button_Search_Click(string key)
        {
            try
            {
                i_Search = 0;
                State = false;
                string s = MusicOnline.Search_Songs(key, 1);
                //Status = "Searching '" + Text_Search.Text + "' . . . ";
                WClient.DownloadStringAsync(new System.Uri(s));
            }
            catch { }
        }

        public void Search_New_Song()
        {
            try
            {
                State = false;
                i_Search = 1;
                string s = URLNewSongs + "1" + ".html";
                WClient.DownloadStringAsync(new System.Uri(s));
            }
            catch { }
        }

        public void Search_Top_Song(short genre)
        {
            try
            {
                State = false;
                i_Search = 2;
                Genre = genre;
                string s = URLSongPage + KeyTop20[genre];
                WClient.DownloadStringAsync(new System.Uri(s));
            }
            catch { }
        }

        private ObservableCollection<MySong> GetTop20Song(string Html, short genre)
        {
            ObservableCollection<MySong> listSongs = new ObservableCollection<MySong>();
            try
            {
                HtmlDocument docPage = new HtmlDocument();
                docPage.LoadHtml(Html);
                HtmlNodeCollection nodePDRank = docPage.DocumentNode.SelectSingleNode("//ul[@class='list_show_chart']").SelectNodes("li");
                if (nodePDRank == null)
                    return null;

                short i = 1;
                foreach (HtmlNode nodeSong in nodePDRank)
                {
                    MySong tempSong = new MySong();
                    HtmlNode nodeInfo = nodeSong.SelectSingleNode("div[@class='box_info_field']").ChildNodes[3];
                    HtmlNodeCollection nodeCollectionA = nodeInfo.SelectNodes(".//a");
                    tempSong.Name = nodeInfo.InnerText;
                    tempSong.URL = nodeInfo.GetAttributeValue("href", "");
                    foreach (HtmlNode n in nodeSong.SelectSingleNode("div[@class='box_info_field']").ChildNodes[5].SelectNodes("a"))
                    {
                        tempSong.Artist.Name += n.InnerText + " ";
                    }

                    switch (genre)
                    {
                        case 0:
                            tempSong.Album.Name = "Top 20 V-Pop";
                            break;
                        case 1:
                            tempSong.Album.Name = "Top 20 UK-US";
                            break;
                        case 2:
                            tempSong.Album.Name = "Top 20 K-Pop";
                            break;
                    }
                    listSongs.Add(tempSong);
                    i++;
                }
                return listSongs;
            }
            catch { return listSongs; }
        }

        private ObservableCollection<MySong> GetListNewSong(string Html)
        {
            ObservableCollection<MySong> listSongs = new ObservableCollection<MySong>();
            try
            {
                HtmlDocument docPage = new HtmlDocument();
                docPage.LoadHtml(Html);
                HtmlNode nodeListSong = docPage.DocumentNode.SelectSingleNode("//div[@class='fram_select']").ChildNodes[4];
                if (nodeListSong == null)
                    return null;
                HtmlNodeCollection nodeSongs = nodeListSong.SelectNodes("li");
                if (nodeSongs == null)
                    return null;
                foreach (HtmlNode nodeSong in nodeSongs)
                {
                    MySong song = new MySong();

                    HtmlNode songName = nodeSong.SelectSingleNode("div[@class='info_song']");
                    HtmlNodeCollection collectionNodesAInSongName = songName.SelectNodes(".//a");

                    song.Name = collectionNodesAInSongName[0].InnerText;
                    song.URL = collectionNodesAInSongName[0].GetAttributeValue("href", "");

                    for (int i = 1; i < collectionNodesAInSongName.Count; i++)
                    {
                        song.Artist.Name += collectionNodesAInSongName[i].InnerText;
                    }

                    song.Album.Name = "Nhac Cua Tui";
                    listSongs.Add(song);
                }
                return listSongs;
            }
            catch { return listSongs; }
        }

        public void GetSongSelected(int index)
        {
            try
            {
                State = false;
                Current_Selected = index;
                string temp = OnL_Current_Songs[index].URL;
                if (OnL_Current_Songs[index].Stream == null)
                    WClient_MoreInfo.DownloadStringAsync(new Uri(temp.Replace("http://www", "http://m")));
                else
                {
                    State = true;
                    LoadingSongCompleted(OnL_Current_Songs[index]);
                }
            }
            catch { }
        }
    }
}
