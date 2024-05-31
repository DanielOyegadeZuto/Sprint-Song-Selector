using Microsoft.AspNetCore.Mvc;
using Song_Selector_app.Services;
using System;
using System.Threading.Tasks;

namespace Song_Selector_app.Controllers
{
    public class SongSelectorController : ControllerBase
    {
        private readonly SpotifyService _spotifyService;

        public SongSelectorController(SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtistInfo()
        {
            string artistId = "4wyNyxs74Ux8UIDopNjIai?si=I2w3dRHeTi2cP1Ag7sErYw";
            string clientId = "f4d2fdef6cb94860962216aec60c73d5";
            string clientSecret = "5aa2c898637f452fa2d89f9f976e0985";

            try
            {
                var artistInfo = await _spotifyService.GetArtistInfo(clientId, clientSecret, artistId);

                // Return artist info in JSON format
                return Ok(new
                {
                    Name = artistInfo.name,
                    Genres = artistInfo.genres,
                    Followers = artistInfo.followers.total
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Failed to retrieve artist information: {ex.Message}");
            }
        }
    }
}