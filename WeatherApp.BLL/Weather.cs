using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherApp.BLL
{
    public class PostApiJson
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public PostApiJson(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
    public class Hourly
    {
        public DateTimeOffset[] Time { get; set; }
        public double[] Temperature2m { get; set; }

        public Hourly(DateTimeOffset[] time, double[] temperature2m)
        {
            Time = time;
            Temperature2m = temperature2m;
        }
    }
    public class HourlyUnits
    {
        public string Temperature2m { get; set; }
        public HourlyUnits(string temperature2m)
        {
            Temperature2m = temperature2m;
        }
    }
    public class CurrentWeather
    {
        public double temperature { get; set; }
        public double windspeed { get; set; }
        public double winddirection { get; set; }
        public int weathercode { get; set; }
        public int is_day { get; set; }
        public DateTimeOffset time { get; set; }

        public CurrentWeather()
        {

        }
        public CurrentWeather(DateTime _time, double _temperature, int _weatherCode, double _windSpeed, int _windDirection)
        {
            time = _time;
            temperature = _temperature;
            weathercode = _weatherCode;
            windspeed = _windSpeed;
            winddirection = _windDirection;
        }
    }
    public class Weather
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public double elevation { get; set; }
        //public Hourly Hourly { get; set; }
        //public HourlyUnits HourlyUnits { get; set; }
        public CurrentWeather current_weather { get; set; }

        const string baseURL = "https://api.open-meteo.com/";
        public async Task<Weather> getWeatherAsync(double lat, double lon)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            // EN - sets dot instead of comma, in order for link to work
            // PL - ustawia kropkê przy wypisnywaniu zamiast przecinka, inaczej api nie zadzia³a.
            // 13,2 -> 13.2

            using (HttpClient client = new HttpClient())
            {
                var weather = new Weather();
                PostApiJson postApiJson = new PostApiJson(lat, lon);
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Clear();

                try
                {
                    HttpResponseMessage response = await client.GetAsync($"v1/forecast?latitude={lat}&longitude={lon}&current_weather=true&timezone=auto");

                    response.EnsureSuccessStatusCode();

                    var empResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(empResponse.ToString());
                    weather = JsonSerializer.Deserialize<Weather>(empResponse);

                    Console.WriteLine(weather.current_weather.temperature);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return weather;
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public Weather()
        {
            timezone = "";
            timezone_abbreviation = "GMT";
            current_weather = new CurrentWeather();
        }
    }
}
