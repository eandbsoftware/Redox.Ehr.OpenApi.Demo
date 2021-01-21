using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System;

namespace Redox.Ehr.Contracts.Models.Dto
{
    /// <summary>
    /// Patient Data Transfer Object
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class PatientDto
    {
        [JsonProperty("MRN", Required = Required.Always)]
        public string MRN { get; set; }

        [JsonProperty("FirstName", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("MiddleName", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string MiddleName { get; set; }

        [JsonProperty("LastName", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("BirthDate", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string BirthDate { get; set; }

        [JsonProperty("Sex", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Sex { get; set; } = "Male";

        [JsonProperty("Email", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("SocialSecurityNumber", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string SocialSecurityNumber { get; set; }

        [JsonProperty("HomePhone", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string HomePhone { get; set; }

        [JsonProperty("MobilePhone", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string MobilePhone { get; set; }

        public AddressDto Address { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}