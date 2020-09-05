using System.Net.Http;
using System.Net.Http.Headers;

namespace DroneDelivery.Api.Tests.Config
{
    public static class TestsExtensions
    {
        public static void AddToken(this HttpClient client, string token)
        {
            client.DefineJsonMediaType()
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static HttpClient DefineJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
