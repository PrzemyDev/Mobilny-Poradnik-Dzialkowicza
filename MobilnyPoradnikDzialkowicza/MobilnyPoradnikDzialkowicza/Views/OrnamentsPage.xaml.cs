using MobilnyPoradnikDzialkowicza.Model;
using MobilnyPoradnikDzialkowicza.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilnyPoradnikDzialkowicza.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrnamentsPage : ContentPage
    {
        PlantsRepository repository = new PlantsRepository();
        public ObservableCollection<PlantsModel> PlantsOrnament { get; set; } = new ObservableCollection<PlantsModel>();
        public OrnamentsPage()
        {
            InitializeComponent();
            try
            {
                foreach (var plant in repository.GetOrnamentPlants())
                {
                    PlantsOrnament.Add(plant);
                }

            }
            catch (Exception e1)
            {
                DisplayAlert("Sqlite ornDB", e1.Message, "OK");
            }
            
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var plant = e.CurrentSelection[0] as PlantsModel;
                if(plant != null)
                {
                    await Navigation.PushAsync(new PlantsDetails(plant));
                    collectionOrnaments.SelectedItem = SelectableItemsView.EmptyViewProperty;
                }
                
            }
            catch (Exception e1)
            {
                await DisplayAlert("X", e1.Message, "X");
            }
            
        }

        private void searchOrnaments_TextChanged(object sender, TextChangedEventArgs e)
        {
            collectionOrnaments.ItemsSource = PlantsOrnament.Where(plant => plant.PlantName.ToLower().Contains(e.NewTextValue.ToLower()));
        }
    }
}