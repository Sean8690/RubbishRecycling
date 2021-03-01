using System;
using InfoTrack.Cdd.Application.Common.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace InfoTrack.Cdd.Infrastructure.Models
{
    /// <summary>
    /// A country or a state/region within a country
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Country : ICountry
    {
        [BsonId]
        public MongoDB.Bson.BsonObjectId Id { get; set; }
        public string Iso2 { get; set; }
        public string Iso3 { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }

        /// <summary>
        /// Continent or greater region (e.g. Oceania, Asia, North America, Middle East)
        /// </summary>
        public string Continent { get; set; }

        /// <summary>
        /// Country or region flag
        /// </summary>
        public Uri FlagUri { get; set; }
    }
}
