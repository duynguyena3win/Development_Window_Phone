using System;
using System.Net;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;

namespace MusicLib
{
    public class SoundCloudDataProvider
    {
        //Endpoint to RESOLVE a track id or playlist id
        const string RESOLVE_URL_ENDPOINT = "http://api.soundcloud.com/resolve.json?url={0}&client_id={1}";

        private Uri _uri;

        public SoundCloudDataProvider(string clientId, string resolveUrl)
        {
            string url = String.Format(RESOLVE_URL_ENDPOINT, resolveUrl, clientId);
            _uri = new Uri(url);
        }

        public async Task<T> LoadTrack<T>()
        {
            try
            {
                string data = await DownloadAsync(_uri);
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SoundCloudDataProvider.LoadTrack: {0}", ex);
                return default(T);
            }
        }


        public async Task<string> DownloadAsync(Uri url)
        {
            HttpClient client = new HttpClient();
            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var response = await client.SendAsync(message))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

    }
}