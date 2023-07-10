using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using GG_ExamenP3.GG_MVVM.Models;
using PropertyChanged;

namespace GG_ExamenP3.GG_MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ClimaViewModel
    {

    
        public string PlaceName { get; set; }
        public DateTime Date { get; set; } =
            DateTime.Now;

        public bool IsLoading { get; set; }

        private HttpClient client;

        public ClimaViewModel()
        {
            client = new HttpClient();
        }


        public ICommand SearchCommand =>
            new Command(async (searchText) =>
            {
                PlaceName = searchText.ToString();
                var location =
                await GetCoordinatesAsync(searchText.ToString());
                await GetWeather(location);
            });

        public GG_ClimaData GG_ClimaData { get; private set; }

        private async Task GetWeather(Location location)
        {
            var url =
                $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&daily=weathercode,temperature_2m_max,temperature_2m_min&current_weather=true&timezone=America%2FChicago";
            IsLoading = true;
            var response =
                await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer
                         .DeserializeAsync<GG_ClimaData>(responseStream);
                    GG_ClimaData = data;

                    for (int i = 0; i < GG_ClimaData.daily.time.Length; i++)
                    {
                        var daily2 = new Daily2
                        {
                            time = GG_ClimaData.daily.time[i],
                            temperature_2m_max = GG_ClimaData.daily.temperature_2m_max[i],
                            temperature_2m_min = GG_ClimaData.daily.temperature_2m_min[i],
                            weathercode = GG_ClimaData.daily.weathercode[i]
                        };
                        GG_ClimaData.daily2.Add(daily2);
                    }
                }
            }
            IsLoading = false;

        }

        private async Task<Location> GetCoordinatesAsync(string address)
        {
            IEnumerable<Location> locations = await Geocoding
                 .Default.GetLocationsAsync(address);

            Location location = locations?.FirstOrDefault();

            if (location != null)
                Console
                     .WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            return location;
        }
    }
}
