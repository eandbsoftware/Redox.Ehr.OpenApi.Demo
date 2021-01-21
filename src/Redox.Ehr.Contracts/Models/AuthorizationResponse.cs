using System;

namespace Redox.Ehr.Contracts.Models
{
    public class AuthorizationResponse
    {
        public string AccessToken { get; set; }

        public DateTime Expires { get; set; }

        public string RefreshToken { get; set; }
    }
}
