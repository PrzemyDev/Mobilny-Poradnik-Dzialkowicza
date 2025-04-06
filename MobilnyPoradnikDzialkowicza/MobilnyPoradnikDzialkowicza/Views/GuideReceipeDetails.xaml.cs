using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilnyPoradnikDzialkowicza.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuideReceipeDetails : ContentPage
    {
        Model.GuidesModel _guidesModel;
        Model.ReceipesModel _receipesModel;
        
        public GuideReceipeDetails(bool isGuide ,Model.GuidesModel guidesModel, Model.ReceipesModel receipesModel)
        {
            InitializeComponent();
            try
            {
                _guidesModel = guidesModel;
                _receipesModel = receipesModel;

                if(isGuide)
                {
                    lblDivOne.Text = "";
                    lblDivTwo.Text = "";
                    Title = _guidesModel.Title;
                    byte[] bytes3 = Convert.FromBase64String(_guidesModel.RecordsImage);
                    MemoryStream ms = new MemoryStream(bytes3);
                    imgRecord.Source = ImageSource.FromStream(() => { return ms; });
                    lblDate.Text = _guidesModel.Date;
                    lblTitle.Text = _guidesModel.Title;
                    lblDescription.Text = TextBinder(_guidesModel.Description);
                    lblDescriptionAdd.Text = TextBinder(_guidesModel.DescriptionAdditional);
                }
                else if (!isGuide)
                {
                    lblDivOne.Text = "Składniki:";
                    lblDivTwo.Text = "Sposób przygotowania:";
                    Title = _receipesModel.Title;
                    byte[] bytes3 = Convert.FromBase64String(_receipesModel.RecordsImage);
                    MemoryStream ms = new MemoryStream(bytes3);
                    imgRecord.Source = ImageSource.FromStream(() => { return ms; });
                    lblDate.Text = _receipesModel.Date;
                    lblTitle.Text = _receipesModel.Title;
                    lblDescription.Text = TextBinder(_receipesModel.Description);
                    lblDescriptionAdd.Text = TextBinder(_receipesModel.DescriptionAdditional);
                }
            }
            catch (Exception exFirebaseDetails)
            {
                DisplayAlert("exFirebaseDetails", exFirebaseDetails.Message, "OK");
            }
        }
        private string TextBinder(string textToUpdate)
        {
            string resultString;
            resultString = textToUpdate.Replace("↓", "\n");
            return resultString;
        }
    }
}