using Flurl.Http;
using Flurl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.OpenApi.Services;
using System;

namespace Redox.Ehr.OpenApi.Extensions
{
    public static class FlurlExtensions
    {
        public static IFlurlRequest WithAuthentication(this Url url)
        {
            return new FlurlRequest(url).WithAuthentication();
        }

        public static IFlurlRequest WithAuthentication(this Uri uri)
        {
            return new FlurlRequest(uri).WithAuthentication();
        }

        public static IFlurlRequest WithAuthentication(this string url)
        {
            return new FlurlRequest(url).WithAuthentication();
        }

        private static IFlurlRequest WithAuthentication(this IFlurlRequest req)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                IAuthService authService = serviceScope.ServiceProvider.GetService<IAuthService>();
                IOptions<RedoxApiConfig> config = (IOptions<RedoxApiConfig>)serviceScope.ServiceProvider.GetService(typeof(IOptions<RedoxApiConfig>));
                // Authenticate user access
                authService.AuthenticateAsync(config.Value.Key, config.Value.Secret).Wait();
                // Add authorization header
                req.WithOAuthBearerToken(authService.AccessToken);
            }
            return req;
        }
    }
}
