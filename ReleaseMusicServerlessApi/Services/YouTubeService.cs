using Newtonsoft.Json;
using ReleaseMusicServerlessApi.Helpers;
using System;
using System.Net.Http;

namespace ReleaseMusicServerlessApi.Services
{
    public class YouTubeService
    {
        private readonly string key = Credentials.keyYtb;

        public YouTubeService()
        {

        }

        public dynamic GetEmbedVideo(string query)
        {
            string queryFormat = string.Format("q={0}&maxResults=1&type=video&part=snippet&videoEmbeddable=true&key={1}", query, this.key);

            dynamic result;

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder("https://www.googleapis.com/youtube/v3/search");
                builder.Query = queryFormat;

                HttpResponseMessage response = client.GetAsync(builder.Uri).Result;

                var responseBody = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(string.Format("{0} ({1}) - {2}", (int)response.StatusCode, response.ReasonPhrase, responseBody));

                result = JsonConvert.DeserializeObject(responseBody);

                return new
                {
                    idEmbed = result.items[0].id.videoId
                };
            }
        }
    }
}
