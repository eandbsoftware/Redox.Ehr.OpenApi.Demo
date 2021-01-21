using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.OpenApi.Extensions;
using System;
using System.Threading.Tasks;

namespace Redox.Ehr.OpenApi.Services
{
    /// <summary>
    /// HTTP Client for accessing Redox API
    /// </summary>
    public class RedoxClient : IRedoxClient
    {
        private readonly RedoxApiConfig _config;
        private readonly ILogger<RedoxClient> _logger;

        public RedoxClient(ILogger<RedoxClient> logger, IOptions<RedoxApiConfig> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        public async Task<T> PostAsync<T>(object data) where T : new()
        {
            _logger.LogDebug(nameof(PostAsync));
            var jsonQuery = GetJsonObject(data);

            var response = await _config.BaseUrl
                .AppendPathSegments(_config.EndpointSegment)
                .WithAuthentication()
                .PostJsonAsync(jsonQuery);

            T result = default;
            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                result = await response.GetJsonAsync<T>();
            }

            return result;
        }

        public async Task<T> QueryAsync<T>(object data) where T : new()
        {
            _logger.LogDebug(nameof(QueryAsync));
            var jsonQuery = GetJsonObject(data);

            var response = await _config.BaseUrl
                .AppendPathSegments(_config.QuerySegment)
                .WithAuthentication()
                .WithTimeout(TimeSpan.FromSeconds(120))
                .PostJsonAsync(jsonQuery);

            T result = default;
            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                result = await response.GetJsonAsync<T>();
            }

            return result;
        }

        private static JObject GetJsonObject(object data)
        {
            // Serialize query to string
            var stringQuery = JsonConvert.SerializeObject(data);

            // Populate Json Object
            var jsonQuery = JObject.Parse(stringQuery);
            return jsonQuery;
        }
    }
}
