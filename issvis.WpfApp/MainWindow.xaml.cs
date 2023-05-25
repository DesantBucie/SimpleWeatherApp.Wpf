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
        public async void loadApiAsync()
        {
            try
            {
                weather = await weather.getWeatherAsync(50.8, 19.7);
                
            }
            catch (Exception ex) { }
            TempLabel.Content = weather.current_weather.temperature.ToString() + "°C";
            LatLabel.Content = "Lat: " + weather.latitude.ToString();
            LonLabel.Content = "Lon: " + weather.longitude.ToString();
            TimezoneLabel.Content = weather.timezone.ToString();
            ArrowAngle.Angle = weather.current_weather.winddirection - 90.0;
            WindDirLabel.Content = "Kierunek wiatru: " + weather.current_weather.winddirection.ToString() + "°";
            WindSpeedLabel.Content = "Prękość wiatru: " + weather.current_weather.windspeed.ToString() + "km/h";

        }
        public MainWindow()
        {
            InitializeComponent();
            loadApiAsync();
        }

        private async void Refresh_click(object sender, RoutedEventArgs e)
        {
            loadApiAsync();
        }

    }
}
