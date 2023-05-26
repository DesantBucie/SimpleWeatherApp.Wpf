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
