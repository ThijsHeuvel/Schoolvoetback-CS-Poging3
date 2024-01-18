using CarDB.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using CarDB.Data;
using CarDB.Exceptions;
using System.Diagnostics.Metrics;

namespace CarDB
{
    internal class Admin
    {
        private static string apiRoot = "https://fifa.amo.rocks/api";
        private static string userKey = "D295372"; // Unique key for retrieving tournament data
        private static TournamentApi data = new TournamentApi();

        public static void ShowMatches()
        {
            Console.Clear();
            Styling.AddHeader("Toon wedstrijden");
            Styling.AddLine();

            List<Match> matches = data.GetTournamentMatches();
            foreach (Match match in matches)
            {
                Console.WriteLine($"{match.Id} | {match.Team1_Name} - {match.Team2_Name}");
            }

            Styling.AddLine();
            Styling.SkipLine();
        }

        public static void ShowMatchResults()
        {
            Console.Clear();
            Styling.AddHeader("Toon wedstrijd resultaten");
            Styling.AddLine();

            List<Result> results = data.GetTournamentResults();
            foreach (Result result in results)
            {
                string winnerStatus;

                if (result.Winner_Id == null)
                {
                    winnerStatus = "Gelijk spel.";
                }
                else
                {
                    if (result.Winner_Id == result.Team1_Id)
                    {
                        winnerStatus = $"{result.Team1_Name} wint!";
                    }
                    else
                    {
                        winnerStatus = $"{result.Team2_Name} wint!";
                    }
                }

                Console.WriteLine($"{result.Id} | {result.Team1_Name} {result.Team1_Score} - {result.Team2_Score} {result.Team2_Name} | {winnerStatus}");
            }

            Styling.AddLine();
            Styling.SkipLine();
        }
    }
}
