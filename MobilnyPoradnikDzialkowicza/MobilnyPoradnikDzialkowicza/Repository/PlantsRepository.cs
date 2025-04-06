using MobilnyPoradnikDzialkowicza.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MobilnyPoradnikDzialkowicza.Repository
{
    public class PlantsRepository
    {
        private readonly SQLiteConnection _database;
        public static string DbPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InjectorStoreTest1.db");
        public PlantsRepository()
        {
            _database = new SQLiteConnection(DbPath);
        }
        public List<PlantsModel> GetAllPlants()
        {
            return _database.Table<PlantsModel>().ToList();
        }
        public List<PlantsModel> GetOrnamentPlants()
        {
            return _database.Table<PlantsModel>().Where(p => p.IsPlantEdible == false).ToList();
        }
        public List<PlantsModel> GetEdiblePlants()
        {
            return _database.Table<PlantsModel>().Where(p => p.IsPlantEdible == true).ToList();
        }
    }
}
