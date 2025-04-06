using Firebase.Database;
using MobilnyPoradnikDzialkowicza.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilnyPoradnikDzialkowicza.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuidesPage : ContentPage
    {
        FirebaseClient firebaseClient = new FirebaseClient("Enter your API key to Firebase database");
        bool isContentDownloaded;

        public GuidesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            TestConnection();
            base.OnAppearing();
        }
        private async void TestConnection()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                var profiles = Connectivity.ConnectionProfiles;
                if ((current == NetworkAccess.Internet) || profiles.Contains(ConnectionProfile.WiFi))
                {
                 
                    if (!isContentDownloaded)
                        GetRecordsFromDatabase();
                }
                else
                {
                    await DisplayAlert("Komunikat", "Brak połączenia z internetem. Jest ono wymagane by móc pobierać wpisy.", "Rozumiem");
                }
            }
            catch (Exception exGetInternet)
            {
                await DisplayAlert("exGetInternet", exGetInternet.Message, "X");
            }
        }
        private async void GetRecordsFromDatabase()
        {
            try
            {
                cvGuidesRecords.ItemsSource = (await firebaseClient.Child("MyDbRecords").OnceAsync<GuidesModel>()).Select(guideRecord => new GuidesModel
                {
                    RecordsImage = guideRecord.Object.RecordsImage,
                    Date = guideRecord.Object.Date,
                    Title = guideRecord.Object.Title,
                    Description = guideRecord.Object.Description,
                    DescriptionAdditional = guideRecord.Object.DescriptionAdditional
                }).Reverse().ToList();
                isContentDownloaded = true;
            }
            catch (Exception exFetchFromDb)
            {
                await DisplayAlert("exFetchFromDb", exFetchFromDb.Message, "OK");
            }
        }

        private async void cvGuidesRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var guide = e.CurrentSelection[0] as GuidesModel;
                if(guide != null)
                {
                    await Navigation.PushAsync(new GuideReceipeDetails(true,guide,null));
                    cvGuidesRecords.SelectedItem = SelectableItemsView.EmptyViewProperty;
                }
            }
            catch (Exception exGuideSelection)
            {
                await DisplayAlert("exGuideSelection", exGuideSelection.Message, "OK");
            }
        }
    }
}