using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Model
{
    internal class Result
    {
        public int Id;
        public int Team1_Id;
        public string Team1_Name;
        public int Team1_Score;
        public int Team2_Id;
        public string Team2_Name;
        public int Team2_Score;
        public int? Winner_Id;

        public Result()
        {
            // Empty constructor (needed for deserialization)
        }

        public Result(int team1_id, string team1_name, int team1_score, int team2_id, string team2_name, int team2_score, int winner_id)
        {
            Team1_Id = team1_id;
            Team1_Name = team1_name;
            Team1_Score = team1_score;
            Team2_Id = team2_id;
            Team2_Name = team2_name;
            Team2_Score = team2_score;
            Winner_Id = winner_id;
        }

        public Result(int id, int team1_id, string team1_name, int team1_score, int team2_id, string team2_name, int team2_score, int winner_id)
        {
            Id = id;
            Team1_Id = team1_id;
            Team1_Name = team1_name;
            Team1_Score = team1_score;
            Team2_Id = team2_id;
            Team2_Name = team2_name;
            Team2_Score = team2_score;
            Winner_Id = winner_id;
        }
    }
}
