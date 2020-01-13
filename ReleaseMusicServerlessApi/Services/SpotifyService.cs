using Newtonsoft.Json;
using ReleaseMusicServerlessApi.Helpers;
using ReleaseMusicServerlessApi.Models;
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

        #region private spotify methods

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

        #region public methods

        public IList<Musics> GetReleasesSpotify(string country)
        {
            dynamic tokenSpotify = this.GenerateToken();
            dynamic result;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.spotify.com/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)tokenSpotify.access_token);

                HttpResponseMessage response = client.GetAsync("v1/browse/new-releases").Result;

                var responseBody = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(string.Format("{0} ({1}) - {2}", (int)response.StatusCode, response.ReasonPhrase, responseBody));

                result = JsonConvert.DeserializeObject(responseBody);
            }

            IList<Musics> musics = this.NormalizeReleasesResponse(result);

            return musics;
        }

        public IList<Musics> GetTracksByQuery(string key, string query, string type)
        {
            dynamic tokenSpotify = this.GenerateToken();
            string queryFormat = string.Format("q={0}:{1}&type={2}", key, query, type);

            dynamic result;

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder("https://api.spotify.com/v1/search");
                builder.Query = queryFormat;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)tokenSpotify.access_token);

                HttpResponseMessage response = client.GetAsync(builder.Uri).Result;

                var responseBody = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(string.Format("{0} ({1}) - {2}", (int)response.StatusCode, response.ReasonPhrase, responseBody));

                result = JsonConvert.DeserializeObject(responseBody);
            }

            IList<Musics> musics;

            switch (type)
            {
                case "track":
                    musics = this.NormalizeTracksResponse(result);
                    return musics;

                case "playlist":
                    musics = this.NormalizePlaylistsResponse(result);
                    return musics;

                case "album":
                    musics = this.NormalizePlaylistsResponse(result);
                    return musics;

                case "artist":
                    musics = this.NormalizePlaylistsResponse(result);
                    return musics;

                default:
                    return new List<Musics>();
            }
        }

        #endregion

        #region normalize responses

        private IList<Musics> NormalizeReleasesResponse(dynamic response)
        {
            IList<Musics> musics = new List<Musics>();

            foreach (var item in response.albums.items)
            {
                musics.Add(new Musics((string)item.name, (string)item.artists[0].name, (string)item.release_date, (string)item.external_urls["spotify"], (string)item.images[1]["url"]));
            }

            return musics;
        }

        private IList<Musics> NormalizeTracksResponse(dynamic response)
        {
            IList<Musics> musics = new List<Musics>();

            foreach (var item in response.tracks.items)
            {
                musics.Add(new Musics((string)item.name, (string)item.artists[0].name, (string)item.album.release_date, (string)item.external_urls["spotify"], (string)item.album.images[1]["url"]));
            }

            return musics;
        }

        private IList<Musics> NormalizePlaylistsResponse(dynamic response)
        {
            IList<Musics> musics = new List<Musics>();

            foreach (var item in response.tracks.items)
            {
                musics.Add(new Musics((string)item.name, (string)item.artists[0].name, (string)item.album.release_date, (string)item.external_urls["spotify"], (string)item.album.images[1]["url"]));
            }

            return musics;
        }

        private IList<Musics> NormalizeAlbumResponse(dynamic response)
        {
            IList<Musics> musics = new List<Musics>();

            foreach (var item in response.tracks.items)
            {
                musics.Add(new Musics((string)item.name, (string)item.artists[0].name, (string)item.album.release_date, (string)item.external_urls["spotify"], (string)item.album.images[1]["url"]));
            }

            return musics;
        }

        private IList<Musics> NormalizeArtistResponse(dynamic response)
        {
            IList<Musics> musics = new List<Musics>();

            foreach (var item in response.tracks.items)
            {
                musics.Add(new Musics((string)item.name, (string)item.artists[0].name, (string)item.album.release_date, (string)item.external_urls["spotify"], (string)item.album.images[1]["url"]));
            }

            return musics;
        }

        #endregion
    }
}
