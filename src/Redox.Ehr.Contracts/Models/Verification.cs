using Newtonsoft.Json;

namespace Redox.Ehr.Contracts.Models
{
    public class Verification
    {
        [JsonProperty("verification-token")]
        public string Verification_token { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }
    }
}
