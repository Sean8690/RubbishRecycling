using System;
using AutoMapper;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos.Order;
using InfoTrack.Cdd.Application.Queries.Orders;
using InfoTrack.Common.Application;
using InfoTrack.Common.Application.Automapper;
using InfoTrack.Platform.Core.Orders.Contracts;
using InfoTrack.Platform.Core.Orders.Contracts.Attributes;

namespace InfoTrack.Cdd.Application.Commands.Order
{
    /// <summary>
    /// Detailed organisation info (Platform.Core Order Request)
    /// </summary>
    [ServiceType(nameof(Common.Enums.ServiceIdentifier.CddOrganisationReport))]
    public class OrganisationOrderRequestCommand : Request,
        ICommand<OrganisationSummaryResponseDto>, IOrderRequest,
        IMapFrom<GetOrdersQuery>, IMatches<OrganisationOrderRequestCommand>
    {
        /// <summary>
        /// Detailed organisation info (Platform.Core Order Request)
        /// </summary>
        public OrganisationOrderRequestCommand() { }

        /// <summary>
        /// Detailed organisation info (Platform.Core Order Request)
        /// </summary>
        public OrganisationOrderRequestCommand(string providerEntityCode, string kyckrCountryCode,
            Common.Enums.ServiceIdentifier serviceIdentifier, string clientReference, string retailerReference, Guid? quoteId)

        {
            ProviderEntityCode = providerEntityCode;
            KyckrCountryCode = kyckrCountryCode;
            ServiceIdentifier = serviceIdentifier;
            RetailerReference = retailerReference;
            ClientReference = clientReference;
            QuoteId = quoteId;
        }

        /// <summary>
        /// Provider's unique organisation identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }

        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        public Common.Enums.ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// QuoteId (optional)
        /// </summary>
        public Guid? QuoteId { get; set; }

#pragma warning disable 1591
        public override string ToString() => ProviderEntityCode;
#pragma warning restore 1591

        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        public override string ToServiceType() => ServiceIdentifier.ToString();

#pragma warning disable 1591
        public void Mapping(Profile profile)
#pragma warning restore 1591
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            profile.CreateMap<GetOrdersQuery, OrganisationOrderRequestCommand>();
        }

        /// <summary>
        /// Shallow "equality" comparer. Ignores QuoteId and all base class properties.
        /// </summary>
        public bool Matches(OrganisationOrderRequestCommand other)
        {
            return other != null &&
                   other.ServiceIdentifier == ServiceIdentifier &&
                   other.KyckrCountryCode == KyckrCountryCode &&
                   other.ProviderEntityCode == ProviderEntityCode;
        }
    }
}
