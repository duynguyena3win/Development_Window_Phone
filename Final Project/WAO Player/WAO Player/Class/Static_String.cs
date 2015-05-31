using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAO_Player.Class
{
    public class Static_String
    {
        public static string[] URLSearch = { "http://www.nhaccuatui.com/tim-nang-cao?title=", "&singer=", "&type=", "&page=" };
        public static string URLImageArtist = "http://mp3.zing.vn/tim-kiem/bai-hat.html?q=";
        public static string Search_Songs(string key, short pageNumber)
        {
            string[] info = key.Split(new char[] { '-' }, 2);
            string songTitle;
            try
            {
                songTitle = info[0];
            }
            catch
            {
                songTitle = "";
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
            return URLSearch[0] + songTitle + URLSearch[1] + singerName + URLSearch[2] + "1" + URLSearch[3] + pageNumber.ToString();
        }
        public const string NHAC_CUA_TUI = "pack://application:,,,/Assets\\NCT_Image.png";
        public const string MP3 = "pack://application:,,,/Assets\\XN.mp3";
    }
    
}
