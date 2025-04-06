using MobilnyPoradnikDzialkowicza.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MobilnyPoradnikDzialkowicza.Repository
{
    public class NotesRepository
    {
        private readonly SQLiteAsyncConnection _database;
        public static string DbPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InjectorStoreTest1.db");
        public NotesRepository()
        {
            _database = new SQLiteAsyncConnection(DbPath);
        }

        public async Task<List<NotesModel>> GetAllNotes()
        {
            var data = await _database.Table<NotesModel>().ToListAsync();
            return data;
        }
        public async Task<int> AddNewNote(NotesModel note)
        {
            return await _database.InsertAsync(note);
        }
        public Task<int> UpdateNote(NotesModel note)
        {
            return _database.UpdateAsync(note);
        }
        public async Task<int> DeleteNote(NotesModel note)
        {
            return await _database.DeleteAsync(note);
        }
    }
}
