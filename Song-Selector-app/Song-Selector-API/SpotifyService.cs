using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Song_Selector_app.Services
{
    public class SpotifyService :ISpotifyService
    {
        private readonly IHttpClientFactory _clientFactory;

        public SpotifyService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<dynamic> GetArtistInfo(string clientId, string clientSecret, string artistId)
        {
            var getAccessToken = new GetAccessTokens(_clientFactory);
            
            string accessToken = await getAccessToken.GetAccessToken(clientId, clientSecret);

            if (!string.IsNullOrEmpty(accessToken))
            {
                string apiUrl = $"https://api.spotify.com/v1/artists/{artistId}";

                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                }
                else
                {
                    throw new HttpRequestException($"Failed to get artist information. Status code: {response.StatusCode}");
                }
            }
            else
            {
                throw new HttpRequestException("Failed to get access token.");
            }
        }
    }
}
