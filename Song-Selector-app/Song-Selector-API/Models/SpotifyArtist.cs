namespace Song_Selector_app.Models
{
    public class SpotifyArtist
    {
        public string Name { get; set; }
        public List<string> Genres { get; set; }
        public FollowersInfo Followers { get; set; }

        public class FollowersInfo
        {
            public int Total { get; set; }
        }
    }
}