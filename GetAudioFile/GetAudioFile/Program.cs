using Newtonsoft.Json;
using System;

using System.Collections.Generic;
using System.Linq;

using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GetAudioFile
{
    class Program
    {
        


        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.assemblyai.com/v2/");
            client.DefaultRequestHeaders.Add("authorization", "ffb38b9629a448c0844badae0a951659");

            string jsonResult = SendFile(client, "C:/Users/mawit/OneDrive/Documents/Hack Kstate 2021/Hack2021/TakeMyBreath.mp3").Result;
            System.Console.WriteLine(jsonResult);
            var jsonObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(jsonResult);

            string url = "";
            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "ffb38b9629a448c0844badae0a951659");

                var json = new { audio_url = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JObject)jsonObj).First).Value).Value };

                StringContent payload = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("https://api.assemblyai.com/v2/transcript", payload);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseJson.GetType());
                var tranID = (Newtonsoft.Json.Linq.JObject) JsonConvert.DeserializeObject(responseJson);
                Console.WriteLine(responseJson);
                url = "https://api.assemblyai.com/v2/transcript/ " + tranID;

                //httpClient.DefaultRequestHeaders.Add("Authorization", "ffb38b9629a448c0844badae0a951659");
                //httpClient.DefaultRequestHeaders.Add("Accepts", "application/json");

                //HttpResponseMessage responseOne = await httpClient.GetAsync("https://api.assemblyai.com/v2/transcript/YOUR-TRANSCRIPT-ID-HERE");
                //responseOne.EnsureSuccessStatusCode();

                //var responseJsonString = await responseOne.Content.ReadAsStringAsync();

            }
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "ffb38b9629a448c0844badae0a951659");
                httpClient.DefaultRequestHeaders.Add("Accepts", "application/json");

                HttpResponseMessage response = await httpClient.GetAsync("https://api.assemblyai.com/v2/transcript/YOUR-TRANSCRIPT-ID-HERE");
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();

            }
        }

        private static async Task<string> SendFile(HttpClient client, string filePath)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "upload");
                request.Headers.Add("Transer-Encoding", "chunked");

                var fileReader = System.IO.File.OpenRead(filePath);
                var streamContent = new StreamContent(fileReader);
                request.Content = streamContent;

                HttpResponseMessage response = await client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

    }
}
