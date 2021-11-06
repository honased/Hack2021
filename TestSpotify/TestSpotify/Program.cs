using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace TestSpotify
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var spotify = new SpotifyClient("BQA31oNZnIUlukMUtZ45ItyNd20kap9TExbUHGBKGTNmsT6dNCZ89yVgUco_-f4Tmcp3Mc9LluyomJ-zjntYGR_dTRacFXVhjj25STqOLQC5RBc_mPR9jfZPxYlsmOzK1jmFzfPybEP_a3bhOgdjV4PhvaE3Otly0qL_Jt5ESVhge8m3Ip5UBSYgGF5FOXGZ5uSy3G70lVd4mZpLWpI");
            SpotifyCommands.Initialize(spotify);
            await Commander.Command("queue waves by kanye west");
            //await SpotifyCommands.SkipTrackNext();
            //await SpotifyCommands.SetPlaying(true);
        }

    }
}
