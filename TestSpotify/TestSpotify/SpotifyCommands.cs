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

        public static async Task SkipSong()
        {
            await client.Player.SkipNext();
        }

        public static async Task QueueSong(string song, string artist = null)
        {
            var tracks = FindSong(song, artist).Result;
            if (tracks.Count <= 0)
            {
                throw new Exception();
            }
            else
            {
                var specificTrack = tracks[0].Uri;
                var request = new PlayerAddToQueueRequest(specificTrack);
                
                request.DeviceId = device;
                await client.Player.AddToQueue(request);
            }
            //await client.Player.AddToQueue();
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

        public static async Task PlayPlaylist(string playlist)
        {
            var obj = await client.Search.Item(new SearchRequest(SearchRequest.Types.Playlist, playlist));
            if(obj.Playlists.Items.Count > 0)
            {
                var actPlaylist = obj.Playlists.Items[0];
                var request = new PlayerResumePlaybackRequest();
                request.ContextUri = actPlaylist.Uri;
                request.DeviceId = device;
                await client.Player.ResumePlayback(request);
            }
        }

        private static int WordDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (string.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (string.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }
    }
}
