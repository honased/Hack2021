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
            var spotify = new SpotifyClient("BQBoZn4C4aWQJnQ_GANP65Y2k78cLQ-kHJu_S9yh9H5jIehim9uCnQVP2rwpy_GeEBeKdbshTH2PLtt4o_DvuVsmMOuXAstiX3kN5N_xDZEE1ttVbNafzhL-_5luJ9_pnQNRPhXsToiE8I6RIdBmWSpfKezjJ2JAWJZ7qYyxedJQovvIi06Qx94knBg_OjNXzx-ih1wI4uIUSzy2tWe_CZzhRogz7qWDTce4CrZGjYdfZb-Uv55jXOE");
            SpotifyCommands.Initialize(spotify);

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
