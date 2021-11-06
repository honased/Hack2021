using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSpotify
{
    public static class Commander
    {
        private enum ParseState
        {
            Initial,
            ReadSong,
            ReadArtist,
            ReadPlaylist
        }

        private enum ParseGoal
        {
            None,
            PlaySong,
            PlayPlaylist
        }

        private struct ParseStruct
        {
            public string song;
            public string artist;
        }

        public static async Task Command(string command)
        {
            string[] tokens = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            ParseState state = ParseState.Initial;
            ParseGoal goal = ParseGoal.None;
            ParseStruct parseData = new ParseStruct();
            StringBuilder builder = new StringBuilder();
            parseData.song = null;
            parseData.artist = null;

            for(int i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i].Trim().ToLower();
                switch(state)
                {
                    case ParseState.Initial:
                        if(token == "play")
                        {
                            if (tokens.Length - i <= 1) return;
                            else if(tokens[i + 1].Trim().ToLower() == "playlist")
                            {
                                state = ParseState.ReadPlaylist;
                                goal = ParseGoal.PlayPlaylist;
                                i += 1;
                            }
                            else
                            {
                                state = ParseState.ReadSong;
                                goal = ParseGoal.PlaySong;
                            }
                        }
                        break;

                    case ParseState.ReadSong:
                        if(token == "by")
                        {
                            if (builder.Length <= 0) return;
                            parseData.song = builder.ToString();
                            builder.Clear();
                            state = ParseState.ReadArtist;
                        }
                        else
                        {
                            builder.Append(token + " ");
                        }
                        break;

                    case ParseState.ReadPlaylist:
                        builder.Append(token + " ");
                        break;

                    case ParseState.ReadArtist:
                        builder.Append(token + " ");
                        break;
                }
            }

            switch(state)
            {
                case ParseState.ReadSong:
                    if (builder.Length == 0) return;
                    parseData.song = builder.ToString().Trim();
                    break;

                case ParseState.ReadArtist:
                    if (builder.Length == 0) return;
                    parseData.artist = builder.ToString().Trim();
                    break;

                case ParseState.ReadPlaylist:
                    if (builder.Length == 0) return;
                    parseData.song = builder.ToString().Trim();
                    break;
            }

            switch(goal)
            {
                case ParseGoal.None:
                    // Do nothing
                    break;

                case ParseGoal.PlaySong:
                    await SpotifyCommands.PlaySong(parseData.song, parseData.artist);
                    break;

                case ParseGoal.PlayPlaylist:
                    await SpotifyCommands.PlayPlaylist(parseData.song);
                    break;
            }
        }
    }
}
