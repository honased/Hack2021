using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace TestSpotify
{
    public static class SpotifyCommands
    {
        private static SpotifyClient client = null;
        private static string device = "";

        public static void Initialize(SpotifyClient sc)
        {
            client = sc;
            var devices = client.Player.GetAvailableDevices().Result;
            if (devices.Devices.Count <= 0) throw new Exception();
            device = devices.Devices[0].Id;
        }

        public static async Task SkipTrackNext()
        {
            await client.Player.SkipNext();
        }

        public static async Task SkipTrackPrevious()
        {
            await client.Player.SkipPrevious();
        }

        public static async Task SetPlaying(bool playing)
        {
            try
            {
                if (playing) await client.Player.ResumePlayback();
                else await client.Player.PausePlayback();
            }
            catch
            {

            }
        }

        public static async Task<List<FullTrack>> FindSong(string song, string artist=null)
        {
            var item = await client.Search.Item(new SearchRequest(SearchRequest.Types.Track, song));
            if (artist != null)
            {
                artist = artist.Trim().ToLower();
                List<FullTrack> returnTracks = new List<FullTrack>();
                foreach(FullTrack ft in item.Tracks.Items)
                {
                    foreach(SimpleArtist sa in ft.Artists)
                    {
                        if(sa.Name.Trim().ToLower().Contains(artist))
                        {
                            returnTracks.Add(ft);
                            break;
                        }
                    }
                }
                return returnTracks;
            }
            else return item.Tracks.Items;
        }

        public static async Task PlaySong(string song, string artist=null)
        {
            var tracks = FindSong(song, artist).Result;
            if(tracks.Count <= 0)
            {
                throw new Exception();
            }
            else
            {
                var specificTrack = tracks[0].Uri;
                var request = new PlayerResumePlaybackRequest();
                request.Uris = new List<string>();
                request.Uris.Add(specificTrack);
                request.DeviceId = device;
                await client.Player.ResumePlayback(request);
            }
        }
    }
}
