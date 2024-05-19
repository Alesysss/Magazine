using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Magazine.Model
{
    [Table("Orders")]
    public class Orders
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int Users_id { get; set; }
        [ManyToOne]
        public Users Users { get; set; }
        [Indexed]
        public int Cart_id { get; set; }
        [ManyToOne]
        public Cart Cart { get; set; }
        public decimal Moneys { get; set; }

        
    }
}
