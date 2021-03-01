// ReSharper disable once CheckNamespace

using System;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie
{
    /// <summary>
    /// Properties which don't come directly from the serialised Frankie API response
    /// </summary>
    public partial class InternationalBusinessProfileResponse
    {
        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }

        /// <summary>
        /// Country name
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Country or region flag
        /// </summary>
        public Uri CountryFlag { get; set; }
    }
}
