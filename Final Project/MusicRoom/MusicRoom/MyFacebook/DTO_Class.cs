using Music_App.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_App.MyFacebook
{
    public static class DTO_Class
    {
        public static MySong Song_Post = new MySong();

        public static void CopySong(MySong Item)
        {
            Song_Post = new MySong(Item);
        }
    }
}
