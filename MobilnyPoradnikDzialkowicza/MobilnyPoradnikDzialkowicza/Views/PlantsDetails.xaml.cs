using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilnyPoradnikDzialkowicza.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlantsDetails : ContentPage
	{
        Model.PlantsModel _plant;
        public PlantsDetails(Model.PlantsModel plant)
		{
			InitializeComponent();
            _plant = plant;
            byte[] bytes2 = Convert.FromBase64String(_plant.PlantImage);
            MemoryStream ms = new MemoryStream(bytes2);
            imgPlantImage.Source = ImageSource.FromStream(() => { return ms; });
            lblPlantName.Text = _plant.PlantName;
            lblPlantDescription.Text = _plant.PlantDescription;
            lblPlantAdditionalOne.Text = _plant.PlantAdditionalOne;
            lblPlantAdditionalTwo.Text = _plant.PlantAdditionalTwo;
            lblPlantAdditionalThree.Text = _plant.PlantAdditionalThree;
        }
    }
}