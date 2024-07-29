using Newtonsoft.Json;
using Song_Selector_app.Models;

namespace Song_Selector_app.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<SpotifyService> _logger;
        private readonly GetAccessTokens _getAccessTokens;

        public SpotifyService(IHttpClientFactory clientFactory, ILogger<SpotifyService> logger, GetAccessTokens getAccessTokens)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _getAccessTokens = getAccessTokens;
        }

        public async Task<SpotifyArtist> GetArtistInfo(string clientId, string clientSecret, string artistId)
        {
            string accessToken = await _getAccessTokens.GetAccessToken(clientId, clientSecret);

            if (!string.IsNullOrEmpty(accessToken))
            {
                string apiUrl = $"https://api.spotify.com/v1/artists/{artistId}";

                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<SpotifyArtist>(jsonResponse);
                }
                else
                {
                    _logger.LogError($"Failed to get artist information. Status code: {response.StatusCode}");
                    throw new HttpRequestException($"Failed to get artist information. Status code: {response.StatusCode}");
                }
            }
            else
            {
                _logger.LogError("Failed to get access token.");
                throw new HttpRequestException("Failed to get access token.");
            }
        }

        public async Task<List<string>> GetArtistSongs(string clientId, string clientSecret, string artistId, int totalLimit = 300)
        {
            string accessToken = await _getAccessTokens.GetAccessToken(clientId, clientSecret);

            if (!string.IsNullOrEmpty(accessToken))
            {
                var songList = new List<string>();
                string apiUrl = $"https://api.spotify.com/v1/artists/{artistId}/albums?include_groups=album,single&market=US&limit=50";
                
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                while (!string.IsNullOrEmpty(apiUrl) && songList.Count < totalLimit)
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                        var albums = data?.items;
                        if (albums == null)
                        {
                            _logger.LogError("Albums data is null.");
                            throw new HttpRequestException("Failed to get artist albums. Albums data is null.");
                        }

                        foreach (var album in albums)
                        {
                            string albumId = album.id;
                            string tracksUrl = $"https://api.spotify.com/v1/albums/{albumId}/tracks?market=US&limit=50";
                            HttpResponseMessage tracksResponse = await client.GetAsync(tracksUrl);

                            if (tracksResponse.IsSuccessStatusCode)
                            {
                                string tracksJsonResponse = await tracksResponse.Content.ReadAsStringAsync();
                                var tracksData = JsonConvert.DeserializeObject<dynamic>(tracksJsonResponse);

                                var tracks = tracksData?.items;
                                if (tracks == null)
                                {
                                    _logger.LogError("Tracks data is null.");
                                    throw new HttpRequestException("Failed to get album tracks. Tracks data is null.");
                                }

                                foreach (var track in tracks)
                                {
                                    if (track?.name != null)
                                    {
                                        songList.Add((string)track.name);
                                    }

                                    if (songList.Count >= totalLimit)
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogError($"Failed to get album tracks. Status code: {tracksResponse.StatusCode}");
                                throw new HttpRequestException($"Failed to get album tracks. Status code: {tracksResponse.StatusCode}");
                            }

                            if (songList.Count >= totalLimit)
                            {
                                break;
                            }
                        }

                        apiUrl = data.next;
                    }
                    else
                    {
                        _logger.LogError($"Failed to get artist albums. Status code: {response.StatusCode}");
                        throw new HttpRequestException($"Failed to get artist albums. Status code: {response.StatusCode}");
                    }
                }

                _logger.LogInformation($"Retrieved {songList.Count} songs before applying the limit.");
                return songList;
            }
            else
            {
                _logger.LogError("Failed to get access token.");
                throw new HttpRequestException("Failed to get access token.");
            }
        }

        public void SaveSongsToCsv(List<string> songs, string filePath)
        {
            File.WriteAllLines(filePath, songs);
        }

        public void SaveSongsToText(List<string> songs, string filePath)
        {
            File.WriteAllLines(filePath, songs);
        }

        public Dictionary<string, string> SaveSongsToDictionary(List<string> songs)
        {
            return songs.ToDictionary(song => song, song => song);
        }

        public List<string> FilterSongsByLetter(List<string> songs, char letter)
        {
            var filteredSongs = songs.Where(song => song.StartsWith(letter.ToString(), System.StringComparison.OrdinalIgnoreCase)).ToList();
            _logger.LogInformation($"Filtered songs count: {filteredSongs.Count} starting with letter {letter}");
            return filteredSongs;
        }
    }
}
