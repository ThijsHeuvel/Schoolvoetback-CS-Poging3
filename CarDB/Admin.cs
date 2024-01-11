using CarDB.Model;
using MySqlConnector.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarDB
{
    internal class Admin
    {
        private static string apiRoot = "https://fifa.amo.rocks/api";
        private static string userKey = "D295372"; // Unique key for retrieving tournament data

        public static void ShowMatches()
        {
            Task<HttpResponseMessage> response = HttpStuff.GetUrl($"{apiRoot}/matches.php?key={userKey}");

            if (response.Result.IsSuccessStatusCode)
            {
                string jsonResult = response.Result.Content.ReadAsStringAsync().Result;

                /*JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                List<Match> matches = JsonConvert.DeserializeObject<List<Match>>(jsonResult, options);

                foreach(Match match in matches)
                {
                    Console.WriteLine($"{match.Id}: {match.Team1_Name} vs {match.Team2_Name}");
                }*/
            }
            else
            {
                Console.WriteLine("Failed to retrieve matches");
            }
        }

        public static void ShowMatchResults()
        {
            // TODO: Show match results
            Console.WriteLine("Show match results");
        }
    }
}
