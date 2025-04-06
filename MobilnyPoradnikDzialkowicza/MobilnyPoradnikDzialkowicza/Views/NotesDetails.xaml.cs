using MobilnyPoradnikDzialkowicza.Model;
using MobilnyPoradnikDzialkowicza.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilnyPoradnikDzialkowicza.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotesDetails : ContentPage
	{
        NotesRepository repository = new NotesRepository();
		public NotesDetails ()
		{
			InitializeComponent();
            gridCreatePage.IsVisible = true;
		}
		
        Model.NotesModel _note;
        public NotesDetails(Model.NotesModel notes)
        {
            InitializeComponent();
            gridEditPage.IsVisible= true;
            _note = notes;
            GetNote(_note);
        }
		private void GetNote(Model.NotesModel notes)
		{
            Title = notes.Title;
            _note = notes;
            lblNotesCreationDate.Text = _note.CreatedDate.ToString();
            lblNotesTitle.Text = _note.Title;
            lblNotesDescription.Text = _note.Description;
        }
        private void EditNote(Model.NotesModel notes)
		{
            _note = notes;
            lblNotesCreationDate.Text = _note.CreatedDate.ToString();
            entrEditTitle.Text = _note.Title;
            edtrEditDescritpion.Text = _note.Description;
        }
        private void EditNote_Clicked(object sender, EventArgs e)
        {
            layoutOptions.IsVisible= false;
            layoutNoteEdition.IsVisible = true;
            EditNote(_note);
        }
        private async void DeleteNote_Clicked(object sender, EventArgs e)
        {
            try
            {
                var deleteAction = await DisplayAlert("Usuń", $"Czy na pewno chcesz usunąć notatkę {_note.Title}?", "Usuń", "Anuluj");
                if (deleteAction)
                {
                    await repository.DeleteNote(_note);
                    await Navigation.PopAsync();
                }
            }
            catch (Exception exceptionDeleteNote)
            {
                await DisplayAlert("Error", exceptionDeleteNote.Message, "OK");
            }
        }
        private async void AcceptEdit_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (entrEditTitle.Text == string.Empty && edtrEditDescritpion.Text == string.Empty)
                {
                    await DisplayAlert("Info", "Przynajmniej jedno z pól musi być wypełnione", "OK");
                }
                else {
                    _note.Title = entrEditTitle.Text;
                    _note.Description = edtrEditDescritpion.Text;
                    await repository.UpdateNote(_note);
                    layoutNoteEdition.IsVisible = false;
                    layoutOptions.IsVisible = true;
                    GetNote(_note);
                }
            }
            catch (Exception exceptionEditNote)
            {
                await DisplayAlert("Error", exceptionEditNote.Message, "FIX");
            }
            
        }

        private void CancelEdit_Clicked(object sender, EventArgs e)
        {
            layoutOptions.IsVisible = true;
            layoutNoteEdition.IsVisible = false;
        }

        private async void AcceptCreate_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (entrNewTitle.Text == null && edtrNewDescritpion.Text == null)
                {
                    await DisplayAlert("Info", "Przynajmniej jedno z pól musi być wypełnione", "OK");
                }
                else { 
                await repository.AddNewNote(new NotesModel
                {
                    Title = entrNewTitle.Text,
                    Description = edtrNewDescritpion.Text,
                    CreatedDate = DateTime.Now,
                });
                entrNewTitle.Text = string.Empty;
                edtrNewDescritpion.Text = string.Empty;

                await Navigation.PopAsync();
                }
            }
             catch (Exception exceptionAddNote)
            {
                await DisplayAlert("Error", exceptionAddNote.Message, "OK");
            }
        }
    }
}