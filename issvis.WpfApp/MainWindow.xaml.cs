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
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
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
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 22, 176, 228));
                    break;
                case 1:
                    s = "Pojedyncze chmury";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 22, 176, 228));
                    break;
                case 2:
                    s = "Częściowe zachmurzenie";
                    break;
                case 3:
                    s = "Pochmurno";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 91, 147, 165));
                    break;
                case 45:
                case 48:
                    s = "Mgliście";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 91, 147, 165));
                    break;
                case 51:
                case 53:
                case 55:
                    s = "Mżawka";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 91, 147, 165));
                    break;
                case 56:
                case 57:
                    s = "Zimna mżawka";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 91, 147, 165));
                    break;
                case 61:
                case 63:
                case 65:
                    s = "Deszcz";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(255, 80, 95, 126));
                    break;
                case 66:
                case 67:
                    s = "Zimny deszcz";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(255, 80, 95, 126));
                    break;
                case 71:
                case 73:
                case 75:
                    s = "Śnieg";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 91, 147, 165));
                    break;
                case 77:
                    s = "Gruby śnieg";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 91, 147, 165));
                    break;
                case 80:
                case 81:
                case 82:
                    s = "Ulewa";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(255, 80, 95, 126));
                    break;
                case 85:
                case 86:
                    s = "Zawieja śnieżna";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(225, 91, 147, 165));
                    break;
                case 95:
                case 96:
                case 99:
                    s = "Burza";
                    MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(255, 80, 95, 126));
                    break;
            }
            WeatherCondLabel.Content = s;
            if (weather.current_weather.is_day == 0)
            {
                MainRectangle.Fill = new SolidColorBrush(Color.FromArgb(255, 92, 107, 138));
            }
        }
        public async Task geoApi(string city = "Czestochowa")
        {
            try
            {
                geocoding = await geocoding.getCordsAsync(city);
                //Console.WriteLine(geocoding.features[0].geometry.type);
                if (geocoding.features.Length != 0)
                {
                    CityLabel.Content = geocoding.features[0].properties.name;
                    await loadApiAsync(geocoding.features[0].geometry.coordinates[1], geocoding.features[0].geometry.coordinates[0]);
                }
                else
                {
                    CityLabel.Content = "Brak takiego miejsca";
                    TempLabel.Content = "-°C";
                    GeoCordsLabel.Content = "0N,";
                    GeoCordsLabel.Content = "0E ";
                    ArrowAngle.Angle = -90.0;
                    WindDirLabel.Content = "Kierunek wiatru: 0°";
                    WindSpeedLabel.Content = "Prękość wiatru: 0 km/h";
                    TimezoneLabel.Content = "UTC0";
                }
            }
            catch (Exception ex) { 
                MessageBox.Show(ex.ToString());
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            geoApi();
        }

        private async void Refresh_click(object sender, RoutedEventArgs e)
        {
            await geoApi(CityLabel.Content.ToString());
        }

        private async void GetCityFromGeo_Click(object sender, RoutedEventArgs e)
        {
            await geoApi(SearchBar.Text);
        }

        private async void OnEnterHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await geoApi(SearchBar.Text);
            }
        }
    }
}
