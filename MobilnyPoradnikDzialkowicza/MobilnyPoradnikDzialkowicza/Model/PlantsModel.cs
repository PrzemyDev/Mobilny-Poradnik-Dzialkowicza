using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilnyPoradnikDzialkowicza.Model
{
    [Table("PlantsModel")]
    public class PlantsModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(64)]
        public bool IsPlantEdible { get; set; }
        public string PlantName { get; set; }
        [MaxLength(2000)]
        public string PlantDescription { get; set; }
        [MaxLength(64)]
        public string PlantAdditionalOne { get; set; }
        [MaxLength(64)]
        public string PlantAdditionalTwo { get; set; }
        [MaxLength(64)]
        public string PlantAdditionalThree { get; set; }

        [MaxLength(5000)]
        public string PlantImage { get; set; }
    }
}