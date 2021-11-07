using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace TestSpotify
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var spotify = new SpotifyClient("BQD4ovKpQrjzxcYHTcB2PrVppcrRQHRv37KtvqQ60yaOTmVbO-5UgKaWjEn9Lk6EUkfz5rRF2d15QV-K0qrNHvTzS3mdBDM1FavNJJ-gLrSIDbbNMVVNhWgkAEqo-nYNllmh78qnVJW2ZVcnlFsQ7XHzHx9Y79ATQzSt700_28zRkPj0v3venVyLQ0bcAqOuyPsQKkeYr2m0mK3Y-WM");
            SpotifyCommands.Initialize(spotify);
            await Commander.Command("skip queue the one unotheactivist");
            //await SpotifyCommands.SkipTrackNext();
            //await SpotifyCommands.SetPlaying(true);
        }

    }
}
