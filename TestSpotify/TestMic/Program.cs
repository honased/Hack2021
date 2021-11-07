using SpotifyAPI.Web;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestSpotify;

namespace TestMic
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            SpotifyCommands.Initialize();

            //await Commander.Command("Play Brexit in America");

            while (true)
            {
                Console.WriteLine("Press enter to issue a command");
                Console.ReadLine();
                await SpotifyCommands.SetPlaying(false);
                string text = await SpeechToText.GetText();
                Console.WriteLine("Command issued: " + text);
                await Commander.Command(text);
            }
        }
    }
}
