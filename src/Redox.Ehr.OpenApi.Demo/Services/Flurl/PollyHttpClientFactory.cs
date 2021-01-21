using Flurl.Http.Configuration;
using System.Net.Http;

namespace Redox.Ehr.OpenApi.Services.Flurl
{
    public class PollyHttpClientFactory : DefaultHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public PollyHttpClientFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return _httpClient;
        }
    }
}
