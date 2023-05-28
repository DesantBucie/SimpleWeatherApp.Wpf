using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherApp.BLL;

namespace SimpleWeather.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Weather weather = new Weather();
        Geocoding geocoding = new Geocoding();
        public async Task loadApiAsync(double lat = 50.8, double lon = 19.7)
        {
            try
            {
                weather = await weather.getWeatherAsync(lat, lon);
                
            }
            catch (Exception ex) { }
            TempLabel.Content = weather.current_weather.temperature.ToString() + "°C";
            GeoCordsLabel.Content = weather.latitude > 0 ? weather.latitude + "N, " : weather.latitude + "S ";
            GeoCordsLabel.Content += weather.longitude > 0 ? weather.longitude + "E " : weather.longitude + "W ";
            ArrowAngle.Angle = weather.current_weather.winddirection - 90.0;
            WindDirLabel.Content = "Kierunek wiatru: " + weather.current_weather.winddirection.ToString() + "°";
            WindSpeedLabel.Content = "Prękość wiatru: " + weather.current_weather.windspeed.ToString() + "km/h";
            TimezoneLabel.Content = weather.timezone.ToString() + ", " + weather.timezone_abbreviation;
            string s = "";
            switch (weather.current_weather.weathercode)
            {
                case 0:
                    s = "Czyste Niebo";
                    break;
                case 1:
                    s = "Pojedyncze chmury";
                    break;
                case 2:
                    s = "Częściowe zachmurzenie";
                    break;
                case 3:
                    s = "Pochmurno";
                    break;
                case 45:
                case 48:
                    s = "Mgliście";
                    break;
                case 51:
                case 53:
                case 55:
                    s = "Mżawka";
                    break;
                case 56:
                case 57:
                    s = "Zimna mżawka";
                    break;
                case 61:
                case 63:
                case 65:
                    s = "Deszcz";
                    break;
                case 66:
                case 67:
                    s = "Zimny deszcz";
                    break;
                case 71:
                case 73:
                case 75:
                    s = "Śnieg";
                    break;
                case 77:
                    s = "Gruby śnieg";
                    break;
                case 80:
                case 81:
                case 82:
                    s = "Ulewa";
                    break;
                case 85:
                case 86:
                    s = "Zawieja śnieżna";
                    break;
                case 95:
                case 96:
                case 99:
                    s = "Burza";
                    break;
            }
            WeatherCondLabel.Content = s;
        }
        public async void geoApi(string city = "Czestochowa")
        {
            try
            {
                geocoding = await geocoding.getCordsAsync(city);
                //Console.WriteLine(geocoding.features[0].geometry.type);
                CityLabel.Content = geocoding.features[0].properties.name;
                await loadApiAsync(geocoding.features[0].geometry.coordinates[1], geocoding.features[0].geometry.coordinates[0]);
            }
            catch (Exception ex) { }
        }
        public MainWindow()
        {
            InitializeComponent();
            geoApi();
            if(weather.current_weather.is_day == 1)
            {
                MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(255, 92, 107, 138));
            }
            else
            {
                MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 22, 176, 228));
            }

        }

        private async void Refresh_click(object sender, RoutedEventArgs e)
        {
            geoApi(CityLabel.Content.ToString());
        }

        private void GetCityFromGeo_Click(object sender, RoutedEventArgs e)
        {
            geoApi(SearchBar.Text);
        }

        private void OnEnterHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                geoApi(SearchBar.Text);
            }
        }
    }
}
