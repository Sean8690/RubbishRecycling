using System;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// A country or a state/region within a country
    /// </summary>
    public interface ICountry
    {
        /// <summary>
        /// Country display name
        /// </summary>
        string CountryName { get; set; }

        /// <summary>
        /// Region display name (if applicable)
        /// </summary>
        string RegionName { get; set; }

        /// <summary>
        /// Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        string KyckrCountryCode { get; set; }

        /// <summary>
        /// ISO2-compliant country code
        /// </summary>
        string Iso2 { get; set; }

        /// <summary>
        /// Continent or greater region (e.g. Oceania, Asia, North America, Middle East)
        /// </summary>
        string Continent { get; set; }

        /// <summary>
        /// Country or region flag
        /// </summary>
        Uri FlagUri { get; set; }
    }
}
