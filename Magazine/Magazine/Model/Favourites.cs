using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Magazine.Model
{
    [Table("Favourites")]
    public class Favourites
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int Users_id { get; set; }
        [ManyToOne]
        public Users Users { get; set; }
        [Indexed]
        public int Product_id { get; set; }
        [ManyToOne]
        public Products Products { get; set; }

        
    }
}
