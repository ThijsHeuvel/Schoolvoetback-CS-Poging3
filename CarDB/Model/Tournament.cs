using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Model
{
    internal class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        //public string Location { get; set; }
        public DateTime Start_time { get; set; }
        public DateTime End_time { get; set; }

        public Tournament(string name, string? description, string team1, string team2, DateTime start_time, DateTime end_time)
        {
            Name = name;
            Description = description;
            Team1 = team1;
            Team2 = team2;
            //Location = location;
            Start_time = start_time;
            End_time = end_time;
        }

        public Tournament(int id, string name, string? description, string team1, string team2, DateTime start_time, DateTime end_time)
        {
            Id = id;
            Name = name;
            Description = description;
            Team1 = team1;
            Team2 = team2;
            //Location = location;
            Start_time = start_time;
            End_time = end_time;
        }

        public override string ToString()
        {
            return $"{Id} | {Name} | {Team1} - {Team2} | {Start_time} - {End_time}";
        }
    }
}
