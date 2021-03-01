using System;
using AutoMapper;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Common.Application.Automapper;

namespace InfoTrack.Cdd.Application.Dtos
{
    /// <summary>
    /// A country or a state/region within a country
    /// </summary>
    public class CountryDto : IMapFrom<Country>, ICountry
    {
        /// <summary>
        /// Country display name
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Region display name (if applicable)
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }

        /// <summary>
        /// For grouping purposes only: ISO2-compliant country code. For non-grouping purposes, please use the KyckrCountryCode instead.
        /// </summary>
        public string Iso2 { get; set; }

        /// <summary>
        /// Continent or greater region (e.g. Oceania, Asia, North America, Middle East)
        /// </summary>
        public string Continent { get; set; }

        /// <summary>
        /// Country or region flag
        /// </summary>
        public Uri FlagUri { get; set; }

        #pragma warning disable 1591
        public void Mapping(Profile profile)
        #pragma warning restore 1591
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            profile.CreateMap<Country, CountryDto>();
        }
    }
}
