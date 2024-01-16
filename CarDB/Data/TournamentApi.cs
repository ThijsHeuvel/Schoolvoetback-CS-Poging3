using CarDB.Exceptions;
using CarDB.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Data
{
    internal class TournamentApi
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiRoot = "https://fifa.amo.rocks/api";
        private static readonly string userKey = "D295372"; // Unique key for retrieving tournament data

        public List<Match> GetTournamentMatches()
        {
            string url = $"{apiRoot}/matches.php?key={userKey}";
            var response = client.GetStringAsync(url).Result;

            // Show the raw response in the debug
            Debug.Write(url);
            Debug.WriteLine(response);

            var matchesDataResponse = JsonConvert.DeserializeObject<List<Match>>(response);

            if (matchesDataResponse == null)
            {
                throw new InvalidResponseException("Tournament matches data response is null");
            }

            return matchesDataResponse;
        }
    }
}
