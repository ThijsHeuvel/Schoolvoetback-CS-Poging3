using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB
{
    internal class HttpStuff
    {
        public static async Task<HttpResponseMessage> GetUrl(string url)
        {
            HttpClient client = new HttpClient();
            var responseTask = await client.GetAsync(url);
            return responseTask;
        }
    }
}
