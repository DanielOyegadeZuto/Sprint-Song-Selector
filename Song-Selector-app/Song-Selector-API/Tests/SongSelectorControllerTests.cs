using Moq;
using NUnit.Framework;
using Song_Selector_app.Controllers;
using Song_Selector_app.Services;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace SongSelector.Tests
{
    [TestFixture]
    public class SongSelectorControllerTests
    {
        private Mock<ISpotifyService> _spotifyServiceMock;
        private SongSelectorController _controller;

        [SetUp]
        public void Setup()
        {
            _spotifyServiceMock = new Mock<ISpotifyService>();
            _controller = new SongSelectorController(_spotifyServiceMock.Object);
        }

        [Test]
        public async Task ArtistSongs_ShouldReturnSongs()
        {
            // Arrange
            string artistId = "artist_id";
            char letter = 'A';
            int limit = 4;
            int totalLimit = 300;
            var songs = new List<string> { "Apple", "Avocado", "Track 1", "Track 2" };

            _spotifyServiceMock.Setup(m => m.GetArtistSongs(It.IsAny<string>(), It.IsAny<string>(), artistId, totalLimit)).ReturnsAsync(songs);
            _spotifyServiceMock.Setup(m => m.FilterSongsByLetter(songs, letter)).Returns(new List<string> { "Apple", "Avocado" });

            // Act
            var result = await _controller.ArtistSongs(artistId, letter, limit, totalLimit) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            var value = result.Value as dynamic;
            ((IEnumerable<string>)value.Songs).Should().Contain(new List<string> { "Apple", "Avocado" });
            ((IEnumerable<string>)value.Songs).Should().NotContain(new List<string> { "Track 1", "Track 2" });
        }
    }
}