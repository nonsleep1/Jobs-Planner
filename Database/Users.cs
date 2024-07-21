using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace Jobs_Planner.Database
{


    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Required, NotNull]
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool isDeleted { get; set; }
       
    }
}
