using MobilnyPoradnikDzialkowicza.Model;
using MobilnyPoradnikDzialkowicza.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilnyPoradnikDzialkowicza.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EdiblesPage : ContentPage
    {
        PlantsRepository repository = new PlantsRepository();
        public ObservableCollection<PlantsModel> PlantsEdible{ get; set; } = new ObservableCollection<PlantsModel>();
        public EdiblesPage()
        {
            InitializeComponent();
            
            try
            {
                foreach (var plant in repository.GetEdiblePlants())
                {
                    PlantsEdible.Add(plant);
                }

            }
            catch (Exception e1)
            {
                Console.Write(e1.Message);
            }

            BindingContext = this;
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
                if (plant != null)
                {
                    await Navigation.PushAsync(new PlantsDetails(plant));
                    collectionEdibles.SelectedItem = SelectableItemsView.EmptyViewProperty;
                }

            }
            catch (Exception e1)
            {
                await DisplayAlert("X", e1.Message, "X");
            }

        }

        private void searchEdibles_TextChanged(object sender, TextChangedEventArgs e)
        {
            collectionEdibles.ItemsSource = PlantsEdible.Where(plant => plant.PlantName.ToLower().Contains(e.NewTextValue.ToLower()));
        }
    }
}