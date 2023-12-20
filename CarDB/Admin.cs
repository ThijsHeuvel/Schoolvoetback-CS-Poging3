using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB
{
    internal class Admin
    {
        static string userKey = "D295372"; // Unique key for retrieving tournament data

        public static void ShowMatches()
        {
            string response = Http.GetUrl($"api/matches.php?key={userKey}");
        }

        public static void ShowMatchResults()
        {
            // TODO: Show match results
            Console.WriteLine("Show match results");
        }
    }
}
