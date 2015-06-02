using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLib
{
    public class SoundCloudMusicService
    {
        //Get your client key at developers.soundcloud.com
        private const string _clientId = "624ef082fca691131d3098d624e41638";

        public static SoundCloudTrackSchema trackModel { get; private set; }

        static public async Task<string> LaunchTrack(string trackUrl)
        {
            try
            {
                var serviceProvider = new SoundCloudDataProvider(_clientId, trackUrl);
                SoundCloudTrackSchema track = await serviceProvider.LoadTrack<SoundCloudTrackSchema>();

                return string.Format("{0}?client_id={1}", track.stream_url, _clientId);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SoundCloudMusicService: {0}", ex.ToString());
                return null;
            }

        }
    }
}
