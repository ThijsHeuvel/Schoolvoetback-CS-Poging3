using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Model
{
    internal class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Team(string name)
        {
            Name = name;
        }

        public Team(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Id} | {Name}";
        }
    }
}
