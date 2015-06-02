using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using DatabaseApps;

namespace WAO_Player.Music_Online_NCT
{
    public class ReadHTML
    {
        public enum eGenre
        {
            NhacTre,
            RapViet,
            KhongLoi,
            NhacTrinh,
            CachMang,
            TienChien,
            TruTinh,
            ThieuNhi,
            RockViet,
            AuMy,
            HanQuoc,
            Nhat,
            Other
        }
        private static string[] KeyTop20 = {"top-20.nhac-viet.html",
                                                 "top-20.au-my.html",
                                                 "top-20.nhac-han.html"};
        private const string URLNewSongs = "http://www.nhaccuatui.com/bai-hat/bai-hat-moi.";
        private const string URLNewALbums = "http://www.nhaccuatui.com/playlist/playlist-moi.html?page=";
        private const string URLSongPage = "http://www.nhaccuatui.com/bai-hat/";
        private const string URLAlbumPage = "http://www.nhaccuatui.com/playlist/";
        private string[] GenreArray = {     "nhac-tre-moi.html",        //0
                                            "rap-viet.html?page=",      //1
                                            "khong-loi.html?page=",     //2
                                            "nhac-trinh.html?page=",    //3
                                            "cach-mang.html?page=",     //4
                                            "tien-chien.html?page=",    //5
                                            "tru-tinh.html?page=",      //6
                                            "thieu-nhi.html?page=",     //7
                                            "rock-viet.html?page=",     //8
                                            "au-my-moi.html",         //9
                                            "han-quoc-moi.html",      //10
                                            "nhac-nhat.html?page=",     //11
                                            "the-loai-khac.html?page="};//12
        private static string URLSearch = "http://www.nhaccuatui.com/tim-kiem/bai-hat?q=";
   
        /// <summary>
        /// Tìm các bài hát trên trang Nhạc Của Tui
        /// </summary>
        /// <param name="key"> Tên bài hát </param>
        /// <param name="by"> by = 1 Tìm theo tên bài hát
        ///                   by = 2 Tìm theo tên ca sĩ trình bày</param>
        /// <param name="pageNumber"> Số thứ tự của trang muốn lấy(bắt đàu từ 0) </param>
        /// <returns> Trả về danh sách các bài hát tìm thấy</returns>
        public List<Song> Search_Songs(string key, short pageNumber)
        {
            List<Song> listSongs = new List<Song>();
            string sourcePage = null;

            sourcePage = SourceWeb.GetWebSource(URLSearch + key + "&page=" + pageNumber.ToString());

            if (sourcePage == null)
                return null;
            listSongs = GetListSearchSong(sourcePage);
            return listSongs;
        }
   
        /// <summary>
        /// Load các bài hát mới từ trang nhạc của tui
        /// </summary>
        /// <param name="pageNumber"> Số thứ tự của trang muốn load(bắt đàu từ 0) </param>
        /// <returns> Trả về danh sách các bái hát mới</returns>
        public List<Song> LoadNewSongs(short pageNumber)
        {
            List<Song> listSongs = new List<Song>();
            string s = URLNewSongs + pageNumber.ToString() + ".html";
            string sourcePage = SourceWeb.GetWebSource(URLNewSongs + pageNumber.ToString() + ".html");
            if (sourcePage != null)
                listSongs = GetListSong(sourcePage);
            return listSongs;
        }
  
        /// <summary>
        /// Load các bài hát trong một thể loại
        /// </summary>
        /// <param name="key"> Thể loại muốn load</param>
        /// <param name="pageNumber"> Trang kết quả muốn lấy (bắt đầu từ 0)</param>
        /// <returns> Trả về danh sách bài hát trong thể loại</returns>
        public List<Song> LoadSongInGenre(eGenre key, short pageNumber)
        {
            List<Song> listSongs = new List<Song>();
            string sourcePage = SourceWeb.GetWebSource(URLSongPage + GenreArray[(int)key] + pageNumber.ToString());
            if (sourcePage == null)
                return null;
            listSongs = GetListSong(sourcePage);
            return listSongs;
        }
   
        /// <summary>
        /// Load các bài hát trong top 20
        /// </summary>
        /// <param name="Genre"> 0 : V-Pop  1: US-UK, 2: K-Pop </param>
        /// <returns> Trả về danh sách các bài hát trong Top 20</returns>
        public List<Song> LoadTop20Songs(short genre /*0 : V-Pop  1: US-UK, 2: K-Pop */)
        {
            List<Song> listSongs = null;
            string s = URLSongPage + KeyTop20[genre];
            string sourcePage = SourceWeb.GetWebSource(URLSongPage + KeyTop20[genre]);
            if (sourcePage == null)
                return null;
            listSongs = GetTop20Song(sourcePage, genre);
            return listSongs;
        }
        /// <summary>
        /// Lấy thêm thông tin (Link Stream, Size, PictureCover)vào đối tượng song của lớp Media.NowPlayingSong
        /// </summary>
        /// <param name="song"> đối tượng được bổ sung thông tin</param>
        public static List<Song> GetMoreSongInfo(string linkPage, out string linkStream, out string lyric, out string linkPicture,
                                           out System.Windows.Media.Imaging.BitmapImage pictureCover, string quality,
                                           out string size, string albumCover, bool isGetSize = false)
        {
            lyric = "";
            size = "";
            linkPicture = "";
            HtmlDocument wap = new HtmlDocument();
            wap.LoadHtml(SourceWeb.GetWebSource(linkPage.Replace("http://www", "http://m")));
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
            //Lấy Stream
            try
            {
                HtmlNode stream = wap.DocumentNode.SelectSingleNode("//div[@class='download']");
                if (quality == null)
                {
                    linkStream = stream.SelectSingleNode(".//a").GetAttributeValue("href", "");
                    if (isGetSize)
                    {
                        WebRequest request;
                        WebResponse reponse;
                        request = WebRequest.Create(linkStream);
                        request.Method = "HEAD";
                        reponse = request.GetResponse();
                        size = Math.Round(reponse.ContentLength * 1.0 / 1024 / 1024, 2).ToString() + "MB";
                        reponse.Close();
                    }
                }
                else if (quality.Contains("128"))
                {
                    linkStream = stream.SelectSingleNode(".//a").GetAttributeValue("href", "");
                    if (isGetSize)
                    {
                        WebRequest request;
                        WebResponse reponse;
                        request = WebRequest.Create(linkStream);
                        request.Method = "HEAD";
                        reponse = request.GetResponse();
                        size = Math.Round(reponse.ContentLength * 1.0 / 1024 / 1024, 2).ToString() + "MB";
                        reponse.Close();
                    }
                }
                else
                {
                    WebRequest request;
                    WebResponse reponse;
                    HtmlNode pdlikeNode = wap.DocumentNode.SelectSingleNode("//div[@class='pdlike']");
                    HtmlNode _blankNode = pdlikeNode.SelectSingleNode(".//a[@target='_blank']");
                    request = WebRequest.Create(_blankNode.GetAttributeValue("href", ""));
                    reponse = request.GetResponse();
                    linkStream = reponse.ResponseUri.ToString();
                    if (linkStream.Contains("login"))
                    {
                        linkStream = stream.SelectSingleNode(".//a").GetAttributeValue("href", "");
                    }
                    //size
                    if (isGetSize)
                    {
                        size = Math.Round(reponse.ContentLength * 1.0 / 1024 / 1024, 2).ToString() + "MB";
                    }
                    reponse.Close();
                }
            }
            catch { linkStream = null; }
            if (albumCover == null)
            {
                pictureCover = new System.Windows.Media.Imaging.BitmapImage(new Uri(/*MY_PACK_URIS.ALBUM_COVER_PICTURE.NHAC_CUA_TUI*/null));
            }
            else
            {
                pictureCover = new System.Windows.Media.Imaging.BitmapImage(new Uri(albumCover));
                linkPicture = albumCover;
            }
            //Lấy các bài hát gợi ý
            HtmlNodeCollection nodeSuggestSongs = wap.DocumentNode.SelectNodes("//div[@id='relatedSong']/div[@class='row bgmusic ']");
            if (nodeSuggestSongs == null)
                return null;
            List<Song> listSuggestSong = new List<Song>();
            foreach (HtmlNode nodeSong in nodeSuggestSongs)
            {
                Song tempSong = new Song();
                HtmlNode nodeArtist = nodeSong.SelectSingleNode(".//p");
                tempSong.Name_Artist = nodeArtist.ChildNodes[1].InnerText;
                HtmlNode nodeTitle = nodeSong.SelectSingleNode(".//h3/a");
                tempSong.Name_Song = nodeTitle.InnerText;
                tempSong.URL = nodeTitle.GetAttributeValue("href", null).Replace("http://m", "http://www");
                tempSong.Name_Album = "Nhac Cua Tui";
                if (tempSong.URL != null)
                    listSuggestSong.Add(tempSong);
            }
            return listSuggestSong;
        }

        public static void GetMoreVideoInfo(string linkPage, out string linkStream, out string size)
        {
            HtmlDocument wap = new HtmlDocument();
            wap.LoadHtml(SourceWeb.GetWebSource(linkPage.Replace("http://www", "http://m")));
            try
            {
                HtmlNode linkStreamNode = wap.DocumentNode.SelectSingleNode("//div[@class='player-video']/div/div/a");
                linkStream = linkStreamNode.GetAttributeValue("href", null);
                WebRequest request;
                WebResponse reponse;
                request = WebRequest.Create(linkStream);
                request.Method = "HEAD";
                reponse = request.GetResponse();
                size = Math.Round(reponse.ContentLength * 1.0 / 1024 / 1024, 2).ToString() + "MB";
                reponse.Close();
            }
            catch { linkStream = null; size = null; }
        }

        private List<Song> GetListSong(string Html)
        {
            List<Song> listSongs = new List<Song>();
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
                Song song = new Song();
                song.Online = true;
                HtmlNode songName = nodeSong.SelectSingleNode("div[@class='info_song']");
                HtmlNodeCollection collectionNodesAInSongName = songName.SelectNodes(".//a");

                song.Quality = "Tốt";

                song.Name_Song = collectionNodesAInSongName[0].InnerText;
                song.URL = collectionNodesAInSongName[0].GetAttributeValue("href", "");

                for (int i = 1; i < collectionNodesAInSongName.Count; i++)
                {
                    song.Name_Artist += collectionNodesAInSongName[i].InnerText;
                }
                song.Top = -1;
                song.Name_Album = "Nhac Cua Tui";
                listSongs.Add(song);
            }
            return listSongs;
        }

        private List<Song> GetListSearchSong(string Html)
        {
            List<Song> listSongs = new List<Song>();
            HtmlDocument docPage = new HtmlDocument();
            docPage.LoadHtml(Html);
            HtmlNode nodeListSong = docPage.DocumentNode.SelectSingleNode("//div[@class='search_returns_frame']").SelectSingleNode("ul");
            if (nodeListSong == null)
                return null;
            HtmlNodeCollection nodeSongs = nodeListSong.SelectNodes("li");
            if (nodeSongs == null)
                return null;
            foreach (HtmlNode nodeSong in nodeSongs)
            {
                Song song = new Song();
                song.Online = true;
                HtmlNode songName = nodeSong.SelectSingleNode("div[@class='info_song']");
                HtmlNodeCollection collectionNodesAInSongName = songName.SelectNodes(".//a");

                song.Quality = "Tốt";

                song.Name_Song = collectionNodesAInSongName[0].InnerText;
                song.URL = collectionNodesAInSongName[0].GetAttributeValue("href", "");

                for (int i = 1; i < collectionNodesAInSongName.Count; i++)
                {
                    song.Name_Artist += collectionNodesAInSongName[i].InnerText;
                }
                song.Top = -1;
                song.Name_Album = "Nhac Cua Tui";
                listSongs.Add(song);
            }
            return listSongs;
        }

        private List<Playlist> GetListPlaylist(string Html)
        {
            List<Playlist> listAlbums = new List<Playlist>();
            HtmlDocument docPage = new HtmlDocument();
            docPage.LoadHtml(Html);
            HtmlNode nodeListAlbum = docPage.DocumentNode.SelectSingleNode("//ul[@class='list-al-pl']");
            if (nodeListAlbum == null)
                return null;
            HtmlNodeCollection nodeAlbums = nodeListAlbum.SelectNodes("li");
            if (nodeAlbums == null)
                return null;
            foreach (HtmlNode nodeAlbum in nodeAlbums)
            {
                Playlist album = new Playlist();
                album.Name_Artist = nodeAlbum.SelectSingleNode("p").SelectSingleNode("a").InnerText;
                album.Top = -1;
                album.Image_Playlist = nodeAlbum.SelectSingleNode("div").SelectSingleNode("a").SelectSingleNode("img").GetAttributeValue("src", "");
                album.ID = nodeAlbum.SelectSingleNode("div").SelectSingleNode("div[@class='user']").SelectSingleNode("p[@class='listent']").GetAttributeValue("id", "");
                album.URL = nodeAlbum.SelectSingleNode("h3").SelectSingleNode("a").GetAttributeValue("href", "");
                album.Name_Playlist = nodeAlbum.SelectSingleNode("h3").SelectSingleNode("a").InnerText;
                listAlbums.Add(album);
            }
            return listAlbums;
        }

        private List<Song> GetTop20Song(string Html, short genre)
        {
            List<Song> listSongs = new List<Song>();
            HtmlDocument docPage = new HtmlDocument();
            docPage.LoadHtml(Html);
            HtmlNodeCollection nodePDRank = docPage.DocumentNode.SelectSingleNode("//ul[@class='list_show_chart']").SelectNodes("li");
            if (nodePDRank == null)
                return null;

            short i = 1;
            foreach (HtmlNode nodeSong in nodePDRank)
            {
                Song tempSong = new Song();
                tempSong.Online = true;
                HtmlNode nodeInfo = nodeSong.SelectSingleNode("div[@class='box_info_field']").ChildNodes[3];
                HtmlNodeCollection nodeCollectionA = nodeInfo.SelectNodes(".//a");
                tempSong.Name_Song = nodeInfo.InnerText;
                tempSong.URL = nodeInfo.GetAttributeValue("href", "");
                foreach (HtmlNode n in nodeSong.SelectSingleNode("div[@class='box_info_field']").ChildNodes[5].SelectNodes("a"))
                {
                    tempSong.Name_Artist += n.InnerText + " ";
                }
                
                tempSong.Top = i;
                tempSong.Quality = "128 kbit/s";
                switch (genre)
                {
                    case 0:
                        tempSong.Name_Album = "Top 20 V-Pop";
                        break;
                    case 1:
                        tempSong.Name_Album = "Top 20 UK-US";
                        break;
                    case 2:
                        tempSong.Name_Album = "Top 20 K-Pop";
                        break;
                }
                listSongs.Add(tempSong);
                i++;
            }
            return listSongs;
        }

        public static List<Song> LoadSongFromPlaylistSelected(Playlist Plist)
        {
            List<Song> lSong = new List<Song>();
            HtmlDocument doc = new HtmlDocument();
            string source = SourceWeb.GetWebSource(Plist.URL.Replace("http://www", "http://m"));
            if (source == null) return null;
            doc.LoadHtml(source);
            HtmlNode playlist = doc.DocumentNode.SelectSingleNode("//div[@id='songList']");
            HtmlNode songnode = playlist.SelectSingleNode("div[@class='pd-playlist']").SelectSingleNode("div[@class='row active']");
            Song playing_song = new Song();
            playing_song.Name_Album = Plist.Name_Playlist;
            playing_song.Name_Artist = Plist.Name_Artist;
            playing_song.Name_Artist = songnode.SelectSingleNode("p").InnerText;
            playing_song.URL = songnode.SelectSingleNode("h3").SelectSingleNode("a").GetAttributeValue("href", "");
            //playing_song.LinkStream = "";
            playing_song.Name_Song = songnode.SelectSingleNode("h3").SelectSingleNode("a").InnerText;
            playing_song.Image_Song = Plist.Image_Playlist;
            lSong.Add(playing_song);
            HtmlNodeCollection songsnode = playlist.SelectSingleNode("div[@class='pd-playlist']").SelectNodes("div[@class='row ']");
            foreach (HtmlNode song in songsnode)
            {
                Song next_song = new Song();
                next_song.Online = true;
                next_song.Name_Album = Plist.Name_Playlist;
                next_song.Name_Artist = Plist.Name_Artist;
                next_song.Name_Artist = song.SelectSingleNode("p").InnerText;
                next_song.URL = song.SelectSingleNode("h3").SelectSingleNode("a").GetAttributeValue("href", "");
                //HtmlDocument next_doc = new HtmlDocument();
                //if (next_doc == null) return null;
                //next_doc.LoadHtml(SourceWeb.GetWebSource(next_song.URL));
                //next_song.LinkStream = "";
                next_song.Name_Song = song.SelectSingleNode("h3").SelectSingleNode("a").InnerText;
                next_song.Image_Song = Plist.Image_Playlist;
                lSong.Add(next_song);
            }
            return lSong;
        }

        public List<Playlist> SearchPlaylists(string key)
        {
            List<Playlist> listPlaylist = new List<Playlist>();
            string sourcePage = null;
            string[] info = key.Split('-');
            string albumTitle;
            try
            {
                albumTitle = info[0];
            }
            catch
            {
                albumTitle = "";
            }

            string singerName;
            try
            {
                singerName = info[1];
            }
            catch
            {
                singerName = "";
            }
            sourcePage = SourceWeb.GetWebSource(URLSearch[0] + albumTitle + URLSearch[1] + singerName + URLSearch[2] + "2" + URLSearch[3] + "1");

            if (sourcePage == null)
                return null;
            listPlaylist = GetListPlaylist(sourcePage);
            return listPlaylist;
        }

        public List<Playlist> LoadNewAlbums(short pagenum)
        {
            List<Playlist> listALbums = new List<Playlist>();
            HtmlDocument doc = new HtmlDocument();
            string contains = SourceWeb.GetWebSource(URLNewALbums + pagenum.ToString());
            if (contains == null) return null;
            doc.LoadHtml(contains);
            HtmlNode new_playlists = doc.DocumentNode.SelectSingleNode("//div[@class='fram_select']").ChildNodes[4];
            if (doc == null) return null;
            HtmlNodeCollection playlists = new_playlists.SelectNodes(".//li");
            foreach (HtmlNode playlist in playlists)
            {
                Playlist album = new Playlist();
                album.URL = playlist.SelectSingleNode(".//div").SelectSingleNode(".//a").GetAttributeValue("href", string.Empty);
                
                album.Top = 0;
                album.Image_Playlist = playlist.ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[0].GetAttributeValue("src", "");
                album.Name_Playlist = playlist.ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerText;
                for (int i = 0; i < playlist.ChildNodes[3].ChildNodes[3].ChildNodes.Count; i++)
                {
                    album.Name_Artist += playlist.ChildNodes[3].ChildNodes[3].ChildNodes[i].InnerText + " ";
                }
                listALbums.Add(album);
            }
            return listALbums;
        }

        public List<Playlist> LoadTop20Albums(short Genre /*1 : V-Pop  2: US-UK, 3: K-Pop */)
        {
            List<Playlist> listALbums = new List<Playlist>();
            HtmlDocument doc = new HtmlDocument();
            string contains = SourceWeb.GetWebSource(URLAlbumPage + GenreArray[Genre]);
            string s = URLAlbumPage + GenreArray[Genre];
            if (contains == null)
                return null;
            doc.LoadHtml(contains);

            HtmlNodeCollection playlists = doc.DocumentNode.SelectSingleNode("//div[@class='fram_select']").SelectNodes(".//li");
            foreach (HtmlNode playlist in playlists)
            {
                try
                {
                    Playlist album = new Playlist();
                    album.URL = playlist.SelectSingleNode(".//div").SelectSingleNode(".//a").GetAttributeValue("href", string.Empty);
                    album.Name_Playlist = playlist.ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerText;
                    album.Image_Playlist = playlist.ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[0].GetAttributeValue("src", "");
                    for (int i = 0; i < playlist.ChildNodes[3].ChildNodes[3].ChildNodes.Count; i++)
                    {
                        album.Name_Artist += playlist.ChildNodes[3].ChildNodes[3].ChildNodes[i].InnerText + " ";
                    }
                    listALbums.Add(album);
                }
                catch { }
            }
            return listALbums;
        }

        private List<Song> GetListVideo(string Html)
        {
            List<Song> listVideos = new List<Song>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNodeCollection nodesLi = doc.DocumentNode.SelectNodes("//ul[@class='list-clip clearfix']/li");
            if (nodesLi == null)
                return null;
            foreach (HtmlNode node in nodesLi)
            {
                Song temp = new Song();
                temp.Online = true;
                HtmlNode nodeImg = node.SelectSingleNode(".//img");
                temp.Image_Song = nodeImg.GetAttributeValue("src", /*MY_PACK_URIS.ALBUM_COVER_PICTURE.NHAC_CUA_TUI*/null);
                HtmlNode nodeTitle = node.SelectSingleNode(".//div/a");
                temp.URL = nodeTitle.GetAttributeValue("href", null);
                temp.Name_Song = nodeTitle.GetAttributeValue("title", "");
                HtmlNode nodeArtist = node.SelectSingleNode("p");
                temp.Name_Artist = "";
                foreach (var a in nodeArtist.ChildNodes)
                {
                    temp.Name_Artist += a.InnerText;
                }
                if (temp.URL != null)
                    listVideos.Add(temp);
            }
            return listVideos;
        }

        public List<Song> SearchVideos(string key, short pageNumber)
        {
            List<Song> listVideos = new List<Song>();
            string sourcePage = null;
            string[] info = key.Split(new char[] { '-' }, 2);
            string videoTitle;
            try
            {
                videoTitle = info[0];
            }
            catch
            {
                videoTitle = "";
            }

            string singerName;
            try
            {
                singerName = info[1];
            }
            catch
            {
                singerName = "";
            }
            sourcePage = SourceWeb.GetWebSource(URLSearch[0] + videoTitle + URLSearch[1] + singerName + URLSearch[2] + "3" + URLSearch[3] + pageNumber.ToString());
            if (sourcePage == null)
                return null;
            listVideos = GetListVideo(sourcePage);
            return listVideos;
        }

        public List<Song> LoadNewVideos(short pageNumber)
        {
            List<Song> listVideos = new List<Song>();
            string sourcePage = null;
            sourcePage = SourceWeb.GetWebSource("http://www.nhaccuatui.com/mv/video-moi.html?page=" + pageNumber.ToString());
            if (sourcePage == null)
                return null;
            listVideos = GetListVideo(sourcePage);
            return listVideos;
        }

        public static Song GetMoreSongInfo(Song Item, bool isGetSize = false)
        {
            string linkStream;
            
            // Lấy image
            HtmlDocument wap = new HtmlDocument();
            wap.LoadHtml(SourceWeb.GetWebSource(Item.URL.Replace("http://www", "http://m")));

            HtmlNode playerSong = wap.DocumentNode.SelectSingleNode("//div[@class='player-song']");
            Item.Name_Song = playerSong.ChildNodes[1].InnerText.Trim();
            Item.Name_Artist = playerSong.ChildNodes[3].ChildNodes[3].InnerText.Trim();
            //Lấy Stream
            try
            {
                HtmlNode stream = wap.DocumentNode.SelectSingleNode("//div[@class='download']");
                if (Item.Quality == null)
                {
                    linkStream = stream.SelectSingleNode(".//a").GetAttributeValue("href", "");
                    if (isGetSize)
                    {
                        WebRequest request;
                        WebResponse reponse;
                        request = WebRequest.Create(linkStream);
                        request.Method = "HEAD";
                        reponse = request.GetResponse();
                        reponse.Close();
                    }
                }
                else if (Item.Quality.Contains("128"))
                {
                    linkStream = stream.SelectSingleNode(".//a").GetAttributeValue("href", "");
                    if (isGetSize)
                    {
                        WebRequest request;
                        WebResponse reponse;
                        request = WebRequest.Create(linkStream);
                        request.Method = "HEAD";
                        reponse = request.GetResponse();
                        reponse.Close();
                    }
                }
                else
                {
                    WebRequest request;
                    WebResponse reponse;
                    HtmlNode pdlikeNode = wap.DocumentNode.SelectSingleNode("//div[@class='pdlike']");
                    HtmlNode _blankNode = pdlikeNode.SelectSingleNode(".//a[@target='_blank']");
                    request = WebRequest.Create(_blankNode.GetAttributeValue("href", ""));
                    reponse = request.GetResponse();
                    linkStream = reponse.ResponseUri.ToString();
                    if (linkStream.Contains("login"))
                    {
                        linkStream = stream.SelectSingleNode(".//a").GetAttributeValue("href", "");
                    }

                    reponse.Close();
                }
            }
            catch { linkStream = null; }

            

            Item.URL = linkStream;
            return Item;
        }


        public static List<Song> GetURLStream(List<Song> Input)
        {
            try
            {
                for (int i = 0; i < Input.Count; i++)
                {
                    Input[i] = GetMoreSongInfo(Input[i]);
                }
                return Input;
            }
            catch
            {
                return null;
            }
        }
    }
}
