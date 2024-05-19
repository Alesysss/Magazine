using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Magazine.Model
{
    [Table("Users")]
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Namee { get; set; }
        public string DateOfBirth { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string Passwordd { get; set; }

       
    }
}
