using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilnyPoradnikDzialkowicza.Model
{
    [Table("NotesModel")]
    public class NotesModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(5000)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }       
    }
}
