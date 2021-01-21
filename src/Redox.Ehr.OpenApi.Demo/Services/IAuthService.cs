using System;
using System.Threading.Tasks;

namespace Redox.Ehr.OpenApi.Services
{
    public interface IAuthService
    {
        string AccessToken { get; set; }

        string RefreshToken { get; set; }

        DateTime? TokenExpires { get; set; }

        Task AuthenticateAsync(string key, string secret);
    }
}