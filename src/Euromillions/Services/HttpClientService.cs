using System.Net.Http;
using System.Threading.Tasks;

namespace Euromillions.Services
{
    public static class HttpClientService
    {
        public static async Task<string> GetAsync(string url)
        {
            var json = string.Empty;

            using (HttpClient httpClient = new HttpClient())
            {
                json = await httpClient.GetStringAsync(url);
            }

            return json;
        }
    }
}
