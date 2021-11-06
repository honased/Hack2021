using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace TestSpotify
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var spotify = new SpotifyClient("BQC9tUo1IZL2iVQVqQVGFl2YpJ4fN3bRBhh_1ywFQ9m6voDxrfGO9D1JFTr2sBYRNjcUMIWAz_g-Y5aPArgLXhIM4gYwCqHY1NGGtz8a4tE5UCVJq43oSYP9uvP7-aTIBS5lRwaQrY7RXbn983Tloyq7o3LjUuGmLN6D62i4fYpTD81xM08iwk8Z6phLuUaSpPE6vfAdxEYu03oAHPPGhdu5NYDmSht0vnl5HSv4Bbzwznwg5AQBBCM");
            SpotifyCommands.Initialize(spotify);
            await Commander.Command("play playlist Bops");
            //await SpotifyCommands.SkipTrackNext();
            //await SpotifyCommands.SetPlaying(true);
        }

    }
}
