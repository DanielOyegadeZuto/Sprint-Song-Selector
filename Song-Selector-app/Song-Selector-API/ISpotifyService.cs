using Song_Selector_app.Models;

namespace Song_Selector_app.Services
{
    public interface ISpotifyService
    {
        Task<SpotifyArtist> GetArtistInfo(string clientId, string clientSecret, string artistId);
        Task<List<string>> GetArtistSongs(string clientId, string clientSecret, string artistId, int limit = 4);
        void SaveSongsToCsv(List<string> songs, string filePath);
        void SaveSongsToText(List<string> songs, string filePath);
        Dictionary<string, string> SaveSongsToDictionary(List<string> songs);
        List<string> FilterSongsByLetter(List<string> songs, char letter);
    }
}