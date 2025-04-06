using Android.Graphics.Drawables;
using MobilnyPoradnikDzialkowicza.Helpers;
using MobilnyPoradnikDzialkowicza.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MobilnyPoradnikDzialkowicza.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForecastPage : ContentPage
    {
        private string Location { get; set; } = "Rzeszów";  //default
        private string Language { get; set; } = "pl";       //default
        public double Longitude { get; set; }               //*długość geo
        public double Latitude { get; set; }                //*szerokość geo
        public ForecastPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            CallCurrentAndForecast();
            base.OnAppearing();
        }
        private async void CallCurrentAndForecast()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                var profiles = Connectivity.ConnectionProfiles;
                if ((current == NetworkAccess.Internet) || profiles.Contains(ConnectionProfile.WiFi))
                {
                    GetCurrentWeather();
                    GetWeatherForecast();
                }
                else
                {
                    await DisplayAlert("Komunikat", "Brak połączenia z internetem. Jest ono wymagane by móc pobierać informacje pogodowe.", "Rozumiem");
                }
            }
            catch (Exception exGetInternet)
            {
                await DisplayAlert("exGetInternet", exGetInternet.Message, "X");
            }
        }
        private async void GetCurrentWeather()
        {
            var urlApi = $"Your API key"; //I used Location and Language variables here
            var resultApi = await WeatherApiCaller.Get(urlApi);
            if (resultApi.Sucessful)
            {
                try
                {
                    var weatherData = JsonConvert.DeserializeObject<CurrentWeather>(resultApi.Response);
                    var date = new DateTime().ToUniversalTime().AddSeconds(weatherData.dt);
                    lblCity.Text = weatherData.name;
                    lblDate.Text = date.ToString("M");
                    lblTemperature.Text = weatherData.main.temp.ToString("0.0");
                    lblTemperatureFeelsLike.Text = weatherData.main.feels_like.ToString("0.0");
                    lblDescription.Text = weatherData.weather[0].description;
                    imgIcon.Source = $"icon{weatherData.weather[0].icon}";
                    lblPressure.Text = $"{weatherData.main.pressure}hpa";
                    lblHumidity.Text = $"{weatherData.main.humidity}%";
					lblCloudiness.Text = $"{weatherData.clouds.all}%";
                    lblWind.Text = $"{weatherData.wind.speed} m/s";
                }
                catch (Exception exGetWeather)
                {
                    await DisplayAlert("GetCurrentWeather", exGetWeather.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("GetCurrentWeather", "Wystąpił problem z wczytywaniem informacji pogodowych", "OK");
            }
        }

        private async void GetWeatherForecast()
        {
            var urlApi = $"Your API key"; //I used Location and Language variables here;
            var resultApi = await WeatherApiCaller.Get(urlApi);
            if (resultApi.Sucessful)
            {
                try
                {
                    var forecastData = JsonConvert.DeserializeObject<ForecastWeather>(resultApi.Response);
                    List<List> listForecasts = new List<List>();

                    var now = DateTime.Now;
                    var tomorrowCetrainHour = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, DateTimeKind.Utc);
                    foreach (var list in forecastData.list)
                    {       //certain date - (check each one by one)
                        var certainDate = DateTime.Parse(list.dt_txt);
                        if ((certainDate > tomorrowCetrainHour
                            && certainDate.Hour == 12)
                            || (certainDate > tomorrowCetrainHour
                            && certainDate.Hour > 00
                            && certainDate.Hour == 21
                            && certainDate.Minute == 00
                            && certainDate.Second == 00))                         
                        {
                            listForecasts.Add(list);
                        }

                    }
                    List<ListedForecast> tempList = new List<ListedForecast>();
                    List<ListedForecast> mergedList = new List<ListedForecast>();
                    int incrementation = 0; 
                    int incrementationMinus = 0;
                    var dateToCompare = DateTime.Now;
                    foreach (var item in listForecasts)
                    {
                        tempList.Add(new ListedForecast
                        {
                            day = DateTime.Parse(item.dt_txt).ToString("dddd"),
                            date = DateTime.Parse(item.dt_txt).ToString("M"),
                            dt_txt_1 = DateTime.Parse(item.dt_txt).ToString("t"),
                            icon = $"icon{item.weather[0].icon}",
                            temperature = item.main.temp.ToString("0.0"),
                            description = item.weather[0].description,
                        });

                        if (DateTime.Parse(item.dt_txt).ToString("dddd") == now.ToString("dddd"))
                        {
                            incrementationMinus = incrementation - 1;
                            tempList[incrementationMinus].dt_txt_2 = $"{DateTime.Parse(item.dt_txt).ToString("t")}";
                            tempList[incrementationMinus].temperature2 = $"{item.main.temp.ToString("0.0")}";
                            tempList[incrementationMinus].description2 = $"{item.weather[0].description}";
                            tempList[incrementationMinus].icon2 = $"icon{item.weather[0].icon}";
                            mergedList.Add(tempList[incrementationMinus]);
                        }

                        now = DateTime.Parse(item.dt_txt);
                        incrementation++;
                    }
                    carouselView.ItemsSource = mergedList;

                }
                catch (Exception exGetForecast)
                {
                    await DisplayAlert("GetForecast", exGetForecast.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("GetForecast", "Wystąpił problem z wczytywaniem informacji pogodowych", "OK");
            }
        }

        class ListedForecast
        {
            public string day { get; set; }
            public string date { get; set; }
            public string dt_txt_1 { get; set; }
            public string dt_txt_2 { get; set; }
            public string icon { get; set; }
            public string icon2 { get; set; }
            public string temperature { get; set; }
            public string temperature2 { get; set; }
            public string description { get; set; }
            public string description2 { get; set; }

        }

        private async void GetLocation()
        {
            try
            {
                var requestGeo = new GeolocationRequest(GeolocationAccuracy.Best);
                var locationGeo = await Geolocation.GetLocationAsync(requestGeo);
                if (locationGeo != null)
                {
                    Longitude = locationGeo.Longitude;
                    Latitude = locationGeo.Latitude;
                    Location = await GetLocality(locationGeo);

                    CallCurrentAndForecast();
                }

            }
            catch (Exception exGPS)
            {
                await DisplayAlert("Lokacja GPS", "Aby pobrać pogodę dla twojej lokalizacji należy włączyć moduł GPS.", "Rozumiem");
                lblDescription.Text = "Wczytaj ponownie pogodę.";
            }
        }
        private async Task<string> GetLocality(Location location)
        {
            try
            {
                var geoPlacemarks = await Geocoding.GetPlacemarksAsync(location);
                var currentPlacemark = geoPlacemarks?.FirstOrDefault();
                if (currentPlacemark != null)
                {
                    return $"{currentPlacemark.Locality},{currentPlacemark.CountryName}";
                }

            }
            catch (Exception exGeolocality)
            {
                await DisplayAlert("exGeolocality", exGeolocality.Message, "OK");
            }
            return null;
        }

        private async void btnSelectManually_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if ((current == NetworkAccess.Internet) || profiles.Contains(ConnectionProfile.WiFi))
            {
                string action = await DisplayActionSheet("Zmień lokację:", "Anuluj", null, "Wpisz ręcznie", "Pobierz z GPS");

                if (action == "Wpisz ręcznie")
                {
                    try
                    {
                        string locationManual = await DisplayPromptAsync("Wprowadź nazwę miejscowości lub kod pocztowy", "np.: 'Rzeszów' lub '35-002'", "Ok", "Anuluj");
                        if ((locationManual != null) && (locationManual != ""))
                        {
                            Location = locationManual.Trim();
                            GetCurrentWeather();
                            GetWeatherForecast();
                        }
                        else if (locationManual == "")
                        {
                            await DisplayAlert("Lokacja", "Upewnij się, że dane lokalizacji zostały podane poprawnie i spróbuj jeszcze raz.", "OK");
                        }

                    }
                    catch (Exception exManualLocationAlert)
                    {
                        await DisplayAlert("exManualLocationAlert", exManualLocationAlert.Message + "Upewnij się, że dane lokalizacji zostały podane poprawnie i spróbuj jeszcze raz.", "OK");
                    }

                }
                else if (action == "Pobierz z GPS")
                {
                    lblDescription.Text = "Wyszukiwanie lokacji...";
                    GetLocation();
                }
            }
            else
            {
                await DisplayAlert("Komunikat", "Brak połączenia z internetem. Jest ono wymagane by móc pobierać informacje pogodowe.", "Rozumiem");
            }
        }
    }
}