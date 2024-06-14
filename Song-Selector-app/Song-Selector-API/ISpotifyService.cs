namespace Song_Selector_app;

public interface ISpotifyService
{
    Task<dynamic> GetArtistInfo(string clientId, string clientSecret, string artistId);
}