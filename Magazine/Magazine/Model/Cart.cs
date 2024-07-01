using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Magazine.Model
{
    [Table("Cart")]
    public class Cart
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
        public decimal Moneys { get; set; }
        public int Quantity { get; set; }

    }
}
