using MauiAppTempoAgora.Models;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;


namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;
            string chave = "a534790a89188b8c49786fb1f18ffe1e";
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())

            {
                HttpResponseMessage resp = await client.GetAsync(url);
                if(resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho =JsonObject.Parse(json);
                    // Deserializa o JSON para o objeto Tempo
                    // t = JsonConvert.DeserializeObject<Tempo>(json);

                    DateTime time= new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                        visibility = (int)rascunho["visibility"],
                        wind_speed = (double)rascunho["wind"]["speed"]

                       

                    }; //fecha objto do tempo
                }//fecha if se o estatus do servidor for sucesso


            }//fecha o using do httpclient
            return t;
        }
    }
}
