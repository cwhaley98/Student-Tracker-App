using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Tracker_App.Schemas
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique] // Ensure usernames are unique
        public string Username { get; set; }

        public string PasswordHash { get; set; } // Store hashed passwords for security, not plain text
    }
}
