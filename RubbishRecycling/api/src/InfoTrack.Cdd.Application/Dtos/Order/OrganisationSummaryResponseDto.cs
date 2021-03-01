using System;
using AutoMapper;
using InfoTrack.Cdd.Application.Commands.Order;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Common.Application.Automapper;

namespace InfoTrack.Cdd.Application.Dtos.Order
{
    /// <summary>
    /// Detailed organisation info
    /// </summary>
    public class OrganisationSummaryResponseDto : OrderDto, IOrganisation,
        IMapFrom<Models.Organisation>,
        IMapFrom<IOrder>,
        IMapFrom<OrganisationOrderRequestCommand>
    {
        /// <summary>
        /// Organisation name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Organisation number, e.g. ABN or ACN
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }

        /// <summary>
        /// Provider's unique organisation identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        #pragma warning disable 1591
        public void Mapping(Profile profile)
        #pragma warning restore 1591
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            profile.CreateMap<IOrganisation, OrganisationSummaryResponseDto>();

            profile.CreateMap<OrganisationOrderRequestCommand, OrganisationSummaryResponseDto>();

            profile.CreateMap<IOrder, OrganisationSummaryResponseDto>();
        }
    }
}
