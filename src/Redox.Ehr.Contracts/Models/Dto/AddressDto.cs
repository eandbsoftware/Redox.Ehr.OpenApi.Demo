using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Redox.Ehr.Contracts.Models.Dto
{
    /// <summary>
    /// Patient Address Data Transfer Object
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class AddressDto
    {
        [JsonProperty("StreetAddress", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string StreetAddress { get; set; }

        [JsonProperty("City", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        [JsonProperty("State", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty("ZipCode", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string ZipCode { get; set; }

        [JsonProperty("County", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string County { get; set; }

        [JsonProperty("Country", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
    }
}


