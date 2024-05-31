using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Song_Selector_app.Services
{
    public class SpotifyService
    {
        private readonly IHttpClientFactory _clientFactory;

        public SpotifyService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<dynamic> GetArtistInfo(string clientId, string clientSecret, string artistId)
        {
            string accessToken = await GetAccessToken(clientId, clientSecret);

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
 
        private async Task<string> GetAccessToken(string clientId, string clientSecret)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            var base64Auth = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Auth);

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"}
            });

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic responseData = JsonConvert.DeserializeObject(responseContent);
                return responseData.access_token;
            }
            else
            {
                return null;
            }
        }
    }
}
