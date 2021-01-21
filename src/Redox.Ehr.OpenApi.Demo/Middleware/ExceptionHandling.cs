using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Redox.Ehr.Core.Errors.Base;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace Redox.Ehr.OpenApi.Middleware
{
    /// <summary>
    /// Global Exception Handling Middleware
    /// </summary>
    public class ExceptionHandling : IMiddleware
    {
        private readonly ILogger _logger;

        public ExceptionHandling(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiLogging>();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseStringAsync();
                _logger.LogError($"Error returned from {ex.Call.Request.Url}: {error}");
                // Handle exception with modifying response with exception details
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Request failed because of exception
                var errorMsg = $"Request Path: {context.Request.Path}, {ex.Message}";
                // Write exception into the log file
                _logger.LogError(errorMsg);
                // Handle exception with modifying response with exception details
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handle exception with modifying response
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="ex">Exception</param>
        /// <returns>Task</returns>
        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var errorMsg = ex.Message;
            if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
            {
                errorMsg = ex.InnerException.Message;
            }
            await context.Response.WriteAsync(new ApiError(context.Response.StatusCode, errorMsg).ToString());
        }
    }
}
