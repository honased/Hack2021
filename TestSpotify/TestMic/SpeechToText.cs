using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TestMic
{
    public static class SpeechToText
    {
        private static ClientWebSocket socket;
        private static ArraySegment<byte> recBytes = new ArraySegment<byte>(new byte[5096*2]);

        public static async Task<string> GetText()
        {
            using (socket = new ClientWebSocket())
            {
                socket.Options.SetRequestHeader("Authorization", "ffb38b9629a448c0844badae0a951659");
                Uri uri = new Uri("wss://api.assemblyai.com/v2/realtime/ws?sample_rate=16000");
                await socket.ConnectAsync(uri, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {
                    //Console.WriteLine("Receiving SessionBegins ...");
                    ArraySegment<byte> sessionBytes = new ArraySegment<byte>(new byte[1024]);
                    var result = await socket.ReceiveAsync(sessionBytes, CancellationToken.None);
                    //Console.WriteLine(Encoding.UTF8.GetString(sessionBytes.Array, 0, result.Count));
                    WaveInEvent waveIn = new WaveInEvent();
                    waveIn.WaveFormat = new WaveFormat(16000, 1);
                    waveIn.DeviceNumber = 2;
                    waveIn.StartRecording();
                    waveIn.DataAvailable += WaveIn_DataAvailable;
                    string text = "";
                    Console.Beep();
                    text = await Task.Run(() => Receive());
                    waveIn.StopRecording();

                    var termObject = new
                    {
                        terminate_session = true
                    };

                    string json = JsonConvert.SerializeObject(termObject);
                    ArraySegment<byte> sendBytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
                    await socket.SendAsync(sendBytes, WebSocketMessageType.Text, true, CancellationToken.None);

                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    return Regex.Replace(text, "[^A-Za-z -]", "");
                }
                return "";
                //Console.WriteLine("Closed");
            }
        }

        private static async Task<string> Receive()
        {
            string returnStr = "";
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(recBytes, CancellationToken.None);
                string json = Encoding.UTF8.GetString(recBytes.Array, 0, result.Count);
                var jobject = (JObject)JsonConvert.DeserializeObject(json);
                string text = ((JValue)jobject["text"]).ToString();
                if (returnStr.Length > 0 && text.Trim().Length == 0) return returnStr;
                else returnStr = text;
            }

            return "";
        }

        private static void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (socket.State == WebSocketState.Open)
            {
                string data = System.Convert.ToBase64String(e.Buffer);
                var serObj = new
                {
                    audio_data = data
                };
                string json = JsonConvert.SerializeObject(serObj);
                ArraySegment<byte> sendBytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
                socket.SendAsync(sendBytes, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
