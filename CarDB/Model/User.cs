using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Model
{
    internal class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Dollars { get; set; }
        public bool IsAdmin { get; set; }

        public User(string username, string password, int dollars, bool isAdmin)
        {
            Username = username;
            Password = password;
            Dollars = dollars;
            IsAdmin = isAdmin;
        }

        public User(int id, string username, string password, int dollars, bool isAdmin)
        {
            Id = id;
            Username = username;
            Password = password;
            Dollars = dollars;
            IsAdmin = isAdmin;
        }
    }

}
