using System;
using AutoMapper;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Common.Application.Automapper;

namespace InfoTrack.Cdd.Application.Dtos
{
    /// <summary>
    /// Fee quote info
    /// </summary>
    public class QuoteDto : IMapFrom<Quote>
    {
        /// <summary>
        /// Fee
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        public Common.Enums.ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }

        /// <summary>
        /// QuoteId
        /// </summary>
        public Guid QuoteId { get; set; }

        #pragma warning disable 1591
        public void Mapping(Profile profile)
        #pragma warning restore 1591
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            profile.CreateMap<Quote, QuoteDto>();
        }
    }
}
