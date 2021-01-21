using Flurl.Http;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.Contracts.Models;
using System.Threading.Tasks;
using System;

namespace Redox.Ehr.OpenApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly RedoxApiConfig _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger, IOptions<RedoxApiConfig> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? TokenExpires { get; set; }

        public async Task AuthenticateAsync(string apiKey, string secret)
        {
            _logger.LogInformation(nameof(AuthenticateAsync));
            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                // Get access token
                await GetAccessTokenAsync(_config.Key, _config.Secret);
            }
            else
            {
                // Check access token expiration
                if (TokenExpires != null && DateTime.Now > TokenExpires)
                {
                    // Get a new one
                    await GetRefreshTokenAsync(_config.Key, RefreshToken);
                }
            }
        }

        private async Task<AuthorizationResponse> GetAccessTokenAsync(string apiKey, string secret)
        {
            _logger.LogInformation(nameof(AuthenticateAsync));
            var result = await _config.BaseUrl
                .AppendPathSegments(_config.AuthenticateSegment)
                .PostUrlEncodedAsync(new
                {
                    apiKey,
                    secret
                })
                .ReceiveJson<AuthorizationResponse>();
            SetProperties(result);
            return result;
        }

        private async Task<AuthorizationResponse> GetRefreshTokenAsync(string apiKey, string refreshToken)
        {
            _logger.LogInformation(nameof(GetRefreshTokenAsync));
            var result = await _config.BaseUrl
                .AppendPathSegments(_config.RefreshTokenSegment)
                .PostUrlEncodedAsync(new
                {
                    apiKey,
                    refreshToken
                })
                .ReceiveJson<AuthorizationResponse>();
            SetProperties(result);
            return result;
        }

        private void SetProperties(AuthorizationResponse result)
        {
            AccessToken = result?.AccessToken;
            TokenExpires = result?.Expires;
            RefreshToken = result?.RefreshToken;
        }
    }
}
