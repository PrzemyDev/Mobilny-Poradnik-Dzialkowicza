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
    public partial class NotesPage : ContentPage
    {
        NotesRepository repository = new NotesRepository();
        List<NotesModel> notes = new List<NotesModel>();
        public async void FetchNotesFromDb()
        {
            List<NotesModel> temporaryList = await repository.GetAllNotes();
            notes = temporaryList;
            notesCollection.ItemsSource = notes;
        }

        public NotesPage()
        {
            InitializeComponent();
 
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                FetchNotesFromDb();
            }
            catch (Exception ex1)
            {
                Console.Write(ex1.Message);
            }
            BindingContext = this;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        private async void NotesCollection_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var note = e.Item as NotesModel;
            await Navigation.PushAsync(new NotesDetails(note));
        }
        private async void btnNewNoteEditor_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotesDetails());
        }
        
    }
}