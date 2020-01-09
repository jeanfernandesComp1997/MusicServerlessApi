using Newtonsoft.Json;
using ReleaseMusicServerlessApi.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ReleaseMusicServerlessApi.Services
{
    public class SpotifyService
    {
        private readonly string authorization = Credentials.authorizationSpotify;
        private const string URL_TOKEN = "https://accounts.spotify.com";

        public SpotifyService()
        {

        }

        #region private methods

        private dynamic GenerateToken()
        {
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_TOKEN);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.authorization);

                var content = new FormUrlEncodedContent(keyValues);

                HttpResponseMessage response = client.PostAsync("/api/token", content).Result;

                var responseBody = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(string.Format("{0} ({1}) - {2}", (int)response.StatusCode, response.ReasonPhrase, responseBody));

                return JsonConvert.DeserializeObject(responseBody);
            }
        }

        #endregion

        public dynamic GetReleasesSpotify(string country)
        {
            dynamic tokenSpotify = this.GenerateToken();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.spotify.com/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)tokenSpotify.access_token);

                HttpResponseMessage response = client.GetAsync("v1/browse/new-releases").Result;

                var responseBody = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(string.Format("{0} ({1}) - {2}", (int)response.StatusCode, response.ReasonPhrase, responseBody));

                return JsonConvert.DeserializeObject(responseBody);
            }
        }
    }
}
