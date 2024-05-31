using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Song_Selector_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongSelectorController : ControllerBase
    {
        // private readonly IHttpClientFactory _clientFactory;

        // public SongSelectorController(IHttpClientFactory clientFactory)
        // {
        //     _clientFactory = clientFactory;
        // }

        [HttpGet]
        public async Task<ActionResult> GetArtistInfo()
        {
            var httpClient = new HttpClient();
            
            string artistId = "4wyNyxs74Ux8UIDopNjIai?si=I2w3dRHeTi2cP1Ag7sErYw";
            string clientId = "f4d2fdef6cb94860962216aec60c73d5";
            string clientSecret = "5aa2c898637f452fa2d89f9f976e0985";

            // Request access token
            string accessToken = await GetAccessToken(clientId, clientSecret);

            // Use the access token to make request to Spotify API
            if (!string.IsNullOrEmpty(accessToken))
            {
                string apiUrl = $"https://api.spotify.com/v1/artists/{artistId}";
                
               httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var artistInfo = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                    // Log artist info to the console
                    Console.WriteLine("Artist Info:");
                    Console.WriteLine($"Name: {artistInfo.name}");
                    Console.WriteLine("Genres:");
                    foreach (var genre in artistInfo.genres)
                    {
                        Console.WriteLine($"- {genre}");
                    }
                    Console.WriteLine($"Followers: {artistInfo.followers.total}");

                    return Ok(); 
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Failed to retrieve artist information. Status code: {response.StatusCode}");
                }
            }
            else
            {
                return BadRequest("Failed to obtain access token.");
            }
        }

        private async Task<string> GetAccessToken(string clientId, string clientSecret)
        {
            // var client = httpClient.CreateClient();
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            var base64Auth = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Auth);

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"}
            });

            var response = await httpClient.SendAsync(request);
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
