using System;
using AutoMapper;
using InfoTrack.Cdd.Application.Commands.PersonReport;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Common.Application.Automapper;
using InfoTrack.Platform.Core.Orders.Contracts;

namespace InfoTrack.Cdd.Application.Models
{
    public class OrderRequest : Request, IMapFrom<PersonOrderReportCommand>, IOrderRequest
    {
        /// <summary>
        /// Provider's unique entity identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        public ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// QuoteId (optional)
        /// </summary>
        public Guid? QuoteId { get; set; }

        public void Mapping(Profile profile)
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            profile.CreateMap<PersonOrderReportCommand, OrderRequest>();
        }
    }
}
