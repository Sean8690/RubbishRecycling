using System;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos.Order;
using InfoTrack.Common.Application;

namespace InfoTrack.Cdd.Application.Commands.Order
{
    /// <summary>
    /// Get detailed info for an organisation matching name/number/country
    /// </summary>
    public class OrganisationOrderCommandHandler : ICommandHandler<OrganisationOrderRequestCommand, OrganisationSummaryResponseDto>
    {
        private readonly IServiceFacade _service;

        /// <summary>
        /// Get detailed info for an organisation matching name/number/country
        /// </summary>
        public OrganisationOrderCommandHandler(IServiceFacade service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        #pragma warning disable 1591
        public async Task<OrganisationSummaryResponseDto> Handle(OrganisationOrderRequestCommand request, CancellationToken cancellationToken)
        #pragma warning restore 1591
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _service.OrderOrganisationSummaryAsync(request, cancellationToken);
        }
    }
}
