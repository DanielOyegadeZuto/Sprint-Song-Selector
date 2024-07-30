using Newtonsoft.Json;


namespace Song_Selector_app
{
    public class GetAccessTokens
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<GetAccessTokens> _logger;

        public GetAccessTokens(IHttpClientFactory clientFactory, ILogger<GetAccessTokens> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public virtual async Task<string> GetAccessToken(string clientId, string clientSecret)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            var base64Auth = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Auth);

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            });

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic responseData = JsonConvert.DeserializeObject(responseContent);
                string accessToken = responseData?.access_token;

                if (accessToken == null)
                {
                    _logger.LogError("Access token is null.");
                    throw new HttpRequestException("Failed to get access token. Access token is null.");
                }

                return accessToken;
            }
            else
            {
                _logger.LogError($"Failed to get access token. Status code: {response.StatusCode}");
                throw new HttpRequestException($"Failed to get access token. Status code: {response.StatusCode}");
            }
        }
    }
}
