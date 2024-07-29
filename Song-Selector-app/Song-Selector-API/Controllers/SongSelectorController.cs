using Microsoft.AspNetCore.Mvc;
using Song_Selector_app.Services;

namespace Song_Selector_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongSelectorController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;

        public SongSelectorController(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        [HttpGet("getArtistSongs")]
        public async Task<IActionResult> ArtistSongs([FromQuery] string artistId, [FromQuery] char? letter = null, [FromQuery] int limit = 4, [FromQuery] int totalLimit = 300)
        {
            string KingKruleArtistId = "4wyNyxs74Ux8UIDopNjIai?si=I2w3dRHeTi2cP1Ag7sErYw";
            string PinkFloydArtistId = "0k17h0D3J5VfsdmQ1iZtE9?si=QshH1dHbQ5qCF5iff0VVyQ";

            string selectedArtistId = artistId ?? KingKruleArtistId; // Default to King Krule if no ID is provided

            string clientId = "f4d2fdef6cb94860962216aec60c73d5";
            string clientSecret = "5aa2c898637f452fa2d89f9f976e0985";

            try
            {
                List<string> songs = await _spotifyService.GetArtistSongs(clientId, clientSecret, selectedArtistId, totalLimit);

                if (songs.Count == 0)
                {
                    return Ok(new { Songs = new List<string>(), Message = "No songs found for the given artist ID." });
                }

                // Filter songs by the specified letter, if provided
                if (letter.HasValue)
                {
                    songs = _spotifyService.FilterSongsByLetter(songs, letter.Value);
                }

                if (songs.Count == 0)
                {
                    return Ok(new { Songs = new List<string>(), Message = $"No songs found starting with the letter '{letter}'." });
                }

                // Return only the first 'limit' songs
                songs = songs.Take(limit).ToList();

                // Return the songs in JSON format
                return Ok(new { Songs = songs });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Failed to retrieve artist songs: {ex.Message}");
            }
        }
    }
}
