using InfoTrack.Cdd.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie
{
    /// <summary>
    /// Frankie: Object to supply the country code, business name and number, along with an optional registry parameter to search for.
    /// </summary>
    public class InternationalBusinessSearchCriteria : IOrganisation
    {
        /// <summary>
        /// Frankie: Name or name fragment you wish to search for.
        /// Note: The less you supply, the more, but less relevant results will be returned.
        /// CRITICAL NOTE: This is NOT to be used as a progressive search function.
        /// You must supply at least one of organisation_name and/or organisation_number.
        /// If you supply both, a name search will be conducted first, then the number will be checked against the result set and any remaining results returned.
        /// </summary>
        [JsonProperty("organisation_name")]
        public string Name { get; set; }

        /// <summary>
        /// Frankie: The business number you wish to search on. This should be a unique corporate identifier as per the country registry you're searching.
        /// </summary>
        [JsonProperty("organisation_number")]
        public string Number { get; set; }

        /// <summary>
        /// Frankie: The ISO 3166-1 alpha2 country code of country registry you wish to search.
        /// This is consistent for all countries except for:
        /// The United States which requires the state registry to query as well.
        /// As an example, for a Delaware query, the country code would be "US-DE".
        /// A Texas query would use "US-TX"
        /// Canada, which also requires you to supply a territory code too.
        /// A Yukon query would use CA-YU, Manitoba would use CA-MB
        /// You can do an all jurisdiction search with CA-ALL
        /// United Arab Emirates (UAE)
        /// For Abu Dhabi, use "DI"
        /// For Dubai, use "DU"
        /// See details here:
        /// https://apidocs.frankiefinancial.com/docs/country-codes-for-international-business-queries
        /// </summary>
        [JsonProperty("Country")]
        public string KyckrCountryCode { get; set; }
    }
}
