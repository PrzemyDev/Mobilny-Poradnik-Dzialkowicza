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
    public partial class ReceipesPage : ContentPage
    {
        FirebaseClient firebaseClient = new FirebaseClient("Enter your API key to Firebase database");
        ObservableCollection<ReceipesModel> receipesRecords { get; set; } = new ObservableCollection<ReceipesModel>();
        public ReceipesPage()
        {
            InitializeComponent();
            BindingContext= this;
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
                    if(receipesRecords.Count <= 0)
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
                var temporaryList = (await firebaseClient.Child("MyDbReceipes").OnceAsync<ReceipesModel>()).Select(receipeRecord => new ReceipesModel
                {
                    RecordsImage = receipeRecord.Object.RecordsImage,
                    Date = receipeRecord.Object.Date,
                    Title = receipeRecord.Object.Title,
                    Description = receipeRecord.Object.Description,
                    DescriptionAdditional = receipeRecord.Object.DescriptionAdditional
                }).ToList();

                foreach (var item in temporaryList)
                {
                    receipesRecords.Add(new ReceipesModel
                    {
                        RecordsImage = item.RecordsImage,
                        Date = item.Date,
                        Title = item.Title, 
                        Description = item.Description,
                        DescriptionAdditional = item.DescriptionAdditional
                    });
                }

                cvRecipesRecords.ItemsSource = receipesRecords.Reverse();
            }
            catch (Exception exFetchFromDb)
            {
                await DisplayAlert("exFetchFromDb", exFetchFromDb.Message, "OK");
            }
        }

        private async void cvRecipesRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                try
                {
                    var receipe = e.CurrentSelection[0] as ReceipesModel;
                    if (receipe != null)
                    {
                        await Navigation.PushAsync(new GuideReceipeDetails(false, null, receipe));
                        cvRecipesRecords.SelectedItem = SelectableItemsView.EmptyViewProperty;
                    }
                }
                catch (Exception exReceipeSelection)
                {
                    await DisplayAlert("exReceipeSelection", exReceipeSelection.Message, "OK");
                }
            }
        private void searchReceipes_TextChanged(object sender, TextChangedEventArgs e)
        {
            cvRecipesRecords.ItemsSource = receipesRecords.Where(receipe => receipe.Title.ToLower().Contains(e.NewTextValue.ToLower()));
        }
    }
}