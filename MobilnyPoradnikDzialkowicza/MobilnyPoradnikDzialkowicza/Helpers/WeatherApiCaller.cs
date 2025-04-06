using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MobilnyPoradnikDzialkowicza.Helpers
{
    public class WeatherApiResponse
    {
        public string ErrMsg { get; set; }
        public string Response { get; set; }
        public bool Sucessful => ErrMsg == null;

    }
    public class WeatherApiCaller
    {
        public static async Task<WeatherApiResponse> Get(string url, string authorizationId = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(authorizationId))
                        httpClient.DefaultRequestHeaders.Add("Authorization", authorizationId);

                var request = await httpClient.GetAsync(url);
                if (request.IsSuccessStatusCode)
                {
                    return new WeatherApiResponse
                    {
                        Response = await request.Content.ReadAsStringAsync()
                    };
                }
                else
                {
                    return new WeatherApiResponse
                    {
                        ErrMsg = request.ReasonPhrase
                    };
                }
            }
        }
    }
}
