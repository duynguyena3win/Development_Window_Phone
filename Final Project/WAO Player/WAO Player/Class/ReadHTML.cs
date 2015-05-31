using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAO_Player.Class;
using HtmlAgilityPack;
using System.Net;
using System.Threading;

namespace WAO_Player.Music_Online_NCT
{
    public class ReadHTML
    {
        public ReadHTML()
        {
        }

        public List<Song_W> GetListSong(string Html)
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

        public Song_W GetMoreSongInfo(string Html, Song_W Item)
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

            Item.Image = Static_String.NHAC_CUA_TUI;

            Item.Stream = linkStream;
            return Item;
        }
    }
}
