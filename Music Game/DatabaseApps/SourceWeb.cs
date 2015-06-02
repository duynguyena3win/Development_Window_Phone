using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WAO_Player.Music_Online_NCT
{
    class SourceWeb
    {
        public static String GetWebSource(String linkURL)
        {
            String source = null;
            try
            {
                WebRequest request;
                WebResponse reponse;
                Stream streamData;
                StreamReader streamReader;

                request = WebRequest.Create(linkURL);
                reponse = request.GetResponse();
                streamData = reponse.GetResponseStream();
                streamReader = new StreamReader(streamData);
                source = streamReader.ReadToEnd();
                reponse.Close();
            }
            catch
            {
            }
            return source;
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
    }
}
