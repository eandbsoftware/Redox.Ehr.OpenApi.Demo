using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.Core.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Redox.Ehr.OpenApi.Middleware
{
    /// <summary>
    /// Middleware for Redox action redirection
    /// </summary>
    public class RedoxDataModelDispatcher : IMiddleware
    {
        private readonly ILogger _logger;
        private readonly RedoxApiConfig _config;

        public RedoxDataModelDispatcher(IOptions<RedoxApiConfig> config, ILoggerFactory loggerFactory)
        {
            _config = config.Value;
            _logger = loggerFactory.CreateLogger<RedoxDataModelDispatcher>();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var path = context.Request.Path.Value;
            _logger.LogInformation($"Request from: {path}");

            // Rewrite to index
            if (path.Contains(_config.RedoxDestinationControllerPath))
            {
                // Ensure the requestBody can be read multiple times
                context.Request.EnableBuffering();

                // Leave the body open so the next middleware can read it.
                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024 * 45,
                    leaveOpen: true);
                var body = await reader.ReadToEndAsync().ConfigureAwait(false);
                var jsonBody = JObject.Parse(body);

                // Redox verification request?
                var verification_token = (string)jsonBody.SelectToken("verification-token");
                if (!string.IsNullOrWhiteSpace(verification_token))
                {
                    // Rewrite request path to navigate to verify destination endpoint
                    context.Request.Path = string.Concat("/", _config.RedoxDestinationControllerPath.AppendToUrl(_config.RedoxDestinationVerificationSegment));
                }
                else
                {
                    // It's a Data Model
                    var dataModel = (string)jsonBody.SelectToken("Meta.DataModel");
                    var eventType = (string)jsonBody.SelectToken("Meta.EventType");
                    if (!string.IsNullOrEmpty(dataModel) && !string.IsNullOrEmpty(eventType))
                    {
                        // Create a controller and method name from DataModel and EventType and rewrite request path
                        // NOTE: Controller/Method must exists!
                        context.Request.Path = string.Concat("/api/", $"{dataModel}/{eventType}");
                    }
                }

                // Reset the request body stream position so the next middleware can read it
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            await next(context).ConfigureAwait(false);
        }
    }
}
