using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Model
{
    internal class TeamPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }

        public TeamPlayer(string name, int teamId)
        {
            Name = name;
            TeamId = teamId;
        }

        public TeamPlayer(int id, string name, int teamId)
        {
            Id = id;
            Name = name;
            TeamId = teamId;
        }

        public override string ToString()
        {
            return $"{Id} | {Name} | {TeamId}";
        }
    }
}
