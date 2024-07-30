using Moq;
using NUnit.Framework;
using Song_Selector_app.Services;
using System.Net;
using FluentAssertions;
using Moq.Protected;
using Newtonsoft.Json;
using Song_Selector_app;

namespace SongSelector.Tests
{
    [TestFixture]
    public class SpotifyServiceTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<ILogger<SpotifyService>> _loggerMock;
        private Mock<GetAccessTokens> _getAccessTokensMock;
        private SpotifyService _spotifyService;

        [SetUp]
        public void Setup()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<SpotifyService>>();
            _getAccessTokensMock = new Mock<GetAccessTokens>(_httpClientFactoryMock.Object, new Mock<ILogger<GetAccessTokens>>().Object) { CallBase = true };

            _spotifyService = new SpotifyService(_httpClientFactoryMock.Object, _loggerMock.Object, _getAccessTokensMock.Object);
        }

        [Test]
        public async Task GetArtistSongs_ShouldReturnLimitedSongs()
        {
            // Arrange
            string clientId = "client_id";
            string clientSecret = "client_secret";
            string artistId = "artist_id";
            int totalLimit = 300;

            var accessToken = "access_token";
            _getAccessTokensMock.Setup(m => m.GetAccessToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(accessToken);

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);

                    if (request.RequestUri.AbsolutePath.Contains("/albums"))
                    {
                        var albumsResponse = new
                        {
                            items = new[]
                            {
                                new { id = "album_id_1" },
                                new { id = "album_id_2" }
                            },
                            next = (string)null
                        };
                        response.Content = new StringContent(JsonConvert.SerializeObject(albumsResponse));
                    }
                    else if (request.RequestUri.AbsolutePath.Contains("/tracks"))
                    {
                        var tracksResponse = new
                        {
                            items = new[]
                            {
                                new { name = "Track 1" },
                                new { name = "Track 2" },
                                new { name = "Track 3" },
                                new { name = "Track 4" },
                                new { name = "Track 5" }
                            }
                        };
                        response.Content = new StringContent(JsonConvert.SerializeObject(tracksResponse));
                    }

                    return response;
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _spotifyService.GetArtistSongs(clientId, clientSecret, artistId, totalLimit);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
        }

        [Test]
        public void FilterSongsByLetter_ShouldReturnFilteredSongs()
        {
            // Arrange
            var songs = new List<string> { "Apple", "Banana", "Avocado", "Blueberry" };
            char letter = 'A';

            // Act
            var result = _spotifyService.FilterSongsByLetter(songs, letter);

            // Assert
            result.Should().Contain(new List<string> { "Apple", "Avocado" });
            result.Should().NotContain(new List<string> { "Banana", "Blueberry" });
        }
    }
}
