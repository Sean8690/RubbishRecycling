using System.Collections.Generic;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared;
using InfoTrack.Cdd.Infrastructure.Utils;
using Newtonsoft.Json;

#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
// ReSharper disable UnusedMember.Global

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie
{
    /// <summary>
    /// Frankie: This wraps the search response details from Kyckr
    /// </summary>
    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class InternationalBusinessSearchResponse : FrankieResponseBase
    {
        /// <summary>
        /// Frankie: 	RequestIDObjectstring($ulid)
        /// example: 01BFJA617JMJXEW6G7TDDXNSHX
        /// Unique identifier for every request.Can be used for tracking down answers with technical support.
        /// Uses the ULID format (a time-based, sortable UUID)
        /// Note: this will be different for every request.
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("ibContinuationKey")]
        public string ContinuationKey { get; set; }

        [JsonProperty("Companies"), JsonWrappedObjectList("CompanyDTO")]
        public List<CompanyDto> Companies { get; set; }
    }

    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class CompanyDto
    {
        [JsonWrappedObjectList("Addresses")]
        public List<AddressDto> Addresses { get; set; }

        public Aliases Aliases { get; set; }

        /// <summary>
        /// Frankie's unique code. We pass this on to the profile search to fetch further company details.
        /// <br /><br />The format of this code varies significantly between countries.
        /// </summary>
        [JsonProperty("Code")]
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// CompanyID e.g. "550594137"
        /// </summary>
        [JsonProperty("CompanyID")]
        public string CompanyNumber { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// e.g. dd/mm/yyyy
        /// </summary>
        public string Date { get; set; }

        public string Function { get; set; }

        /// <summary>
        /// e.g. "APTY" or "NZ Limited Company" (varies significantly between countries)
        /// </summary>
        public string LegalForm { get; set; }

        /// <summary>
        /// e.g. "REGD" or "active" or "Registered" (varies significantly between countries)
        /// </summary>
        public string LegalStatus { get; set; }

        public string MoreKey { get; set; }

        public bool? Official { get; set; }

        /// <summary>
        /// e.g. "NZBN"
        /// </summary>
        public string RegistrationAuthority { get; set; }

        public string RegistrationAuthorityCode { get; set; }

        public string Source { get; set; }

        [JsonProperty("VirtualID")]
        public string VirtualId { get; set; }
    }
}
