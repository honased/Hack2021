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

        public static void Initialize(SpotifyClient sc=null)
        {
            client = sc;
            if(client == null)
            {
                client = new SpotifyClient("BQDFaH1UFj98hT6Nnw2bvCP61w6krjtvn5r_wz1iS1vWzH-LGjuTmXBkPu-iuXu8AA_Y2RRqRMb9sGyVi8it6bMZslSZNkfa6b5gP2Vfl8u-WI2zpU1wnwF53-viv7e4tUhrPLEuZ9T6fJrkMWEoR28l_kt0AXAFovE32gzkk4YMUm6YcNDWDWlwXlxduIIAHlPmqA8nADyy0Uy5m3IKdWkHLswzPw0aAI7W_mTZCvF6bPLrgwZ2Vno");
            }
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

        public static async Task SetProgress(int progress)
        {
            await client.Player.SeekTo(new PlayerSeekToRequest(progress));
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
                List<FullTrack> returnTracks;
                returnTracks = item.Tracks.Items.OrderBy(x => WordDistance(x.Artists.Aggregate((y, z) => (WordDistance(y.Name.ToLower(), artist) < WordDistance(z.Name.ToLower(), artist) ? y : z)).Name.ToLower(), artist)).ToList();
                return returnTracks;
            }
            else return item.Tracks.Items;
        }

        public static async Task PlaySong(string song, string artist=null)
        {
            var tracks = FindSong(song, artist).Result;
            if(tracks.Count <= 0)
            {
                return;
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

        public static async Task<CurrentlyPlaying> GetCurrentStatus()
        {
            var current = await client.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
            return current;
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
