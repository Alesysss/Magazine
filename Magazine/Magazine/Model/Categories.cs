using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Magazine.Model
{
    [Table("Categories")]
    public class Categories
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 
        public string Namee { get; set; }

       
    }

}
