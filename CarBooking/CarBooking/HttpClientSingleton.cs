using System.Net.Http;

namespace CarBooking
{
    public class HttpClientSingleton
    {
        private static HttpClient client;

        public static HttpClient GetClient()
        {
            if (client is null)
            {
                client = new HttpClient();
            }

            return client;
        }
    }
}
