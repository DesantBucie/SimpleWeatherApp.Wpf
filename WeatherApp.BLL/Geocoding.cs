using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace WeatherApp.BLL
{
    public class Properties
    {
        public string country { get; set; }
        public string countrycode { get; set; }
        public string name { get; set; }
        public string type { get; set; }

        public Properties()
        {
            country = "";
            countrycode = "";
            name = "";
            type = "";
        }
    }
    public class Geometry
    {
        public double[] coordinates { get; set; }
        public string type { get; set; }

        public Geometry()
        {
            coordinates = new double[2];
            type = "";
        }
    } 
    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }

        public Feature()
        {
            properties = new Properties();
            geometry = new Geometry();
            type = "";
        }
    }
    public class Geocoding
    {
        public string type { get; set; }
        public Feature[] features;
        public Geocoding()
        {
            features = new Feature[5];
            type = "";
        }
        const string baseURL = "https://photon.komoot.io/api/";
        public async Task<Geocoding> getCordsAsync(string city)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            // EN - sets dot instead of comma, in order for link to work
            // PL - ustawia kropkê przy wypisnywaniu jako przecinek, inaczej api nie zadzia³a.

            using (HttpClient client = new HttpClient())
            {
                var geo = new Geocoding();
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Clear();

                try
                {
                    HttpResponseMessage response = await client.GetAsync($"?q={city}&limit=1");

                    response.EnsureSuccessStatusCode();

                    var empResponse = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(empResponse.ToString());
                    //geo = JsonSerializer.Deserialize<Geocoding>(empResponse);
                    geo = JsonConvert.DeserializeObject<Geocoding>(empResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return geo;
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
    }
}
