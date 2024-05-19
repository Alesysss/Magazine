using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Model
{
    [Table("Products")]
    public class Products
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Namee { get; set; }
        public decimal Price { get; set; }
        public string Descriptionn { get; set; }
        [Indexed]
        public int Category_id { get; set; }
        [ManyToOne]
        public Categories Categories { get; set; }
        public string Foto { get; set; }

    }
}
