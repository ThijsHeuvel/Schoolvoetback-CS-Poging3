using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Model
{
    internal class Match
    {
        public int Id { get; set; }
        public int Team1_Id { get; set; }
        public int Team2_Id { get; set; }
        public string Team1_Name { get; set; }
        public string Team2_Name { get; set; }

        public Match(int team1_id, int team2_id, string team1_name, string team2_name)
        {
            Team1_Id = team1_id;
            Team2_Id = team2_id;
            Team1_Name = team1_name;
            Team2_Name = team2_name;
        }

        public Match(int id, int team1_id, int team2_id, string team1_name, string team2_name)
        {
            Id = id;
            Team1_Id = team1_id;
            Team2_Id = team2_id;
            Team1_Name = team1_name;
            Team2_Name = team2_name;
        }
    }
}
