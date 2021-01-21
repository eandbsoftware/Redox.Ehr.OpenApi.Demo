using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redox.DataModel.Builder.Configuration;

namespace Redox.Ehr.OpenApi.Controllers.Base
{
    // Inject common services in a BaseController
    public abstract class BaseController<T> : ControllerBase where T: BaseController<T>
	{
		private ILogger<T> _logger;

		protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();

		/// <summary>
		/// Verify token from Redox  
		/// </summary>
		/// <returns>True/False</returns>
		protected bool IsAuthenticated()
		{
			var config = HttpContext.RequestServices.GetService<IOptions<RedoxApiConfig>>().Value;
			return Request.Headers.TryGetValue("verification-token", out var headerValue) && headerValue.Equals(config.RedoxDestinationVerificationToken);
		}
	}
}
