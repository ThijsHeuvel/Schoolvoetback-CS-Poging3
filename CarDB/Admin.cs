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
            List<Match> matches = data.GetTournamentMatches();
            foreach (Match match in matches)
            {
                Console.WriteLine(match);
            }
        }

        public static void ShowMatchResults()
        {
            // TODO: Show match results
            Console.WriteLine("TODO: Show match results");
        }
    }
}
