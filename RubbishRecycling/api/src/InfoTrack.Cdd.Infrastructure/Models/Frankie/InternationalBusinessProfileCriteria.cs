using Newtonsoft.Json;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie
{
    /// <summary>
    /// Frankie: Object to supply the country code and company code whose details you wish to retrieve.
    /// </summary>
    public class InternationalBusinessProfileCriteria
    {
        /// <summary>
        /// The ISO 3166-1 alpha2 country code of country registry you wish to search.
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
        [JsonProperty("country")]
        public string KyckrCountryCode { get; set; }

        /// <summary>
        /// Frankie: This is the company number returned in the search results
        /// (InternationalBusinessSearchResponse.Companies.CompanyDTO[n].Code)
        /// </summary>
        [JsonProperty("company_code")]
        public string ProviderEntityCode { get; set; }
    }
}
