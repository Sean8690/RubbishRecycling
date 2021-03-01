using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Commands.Order;
using InfoTrack.Cdd.Application.Commands.PersonOrder;
using InfoTrack.Cdd.Application.Commands.PersonReport;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Cdd.Application.Dtos.Order;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Order service facade
    /// </summary>
    public interface IServiceFacade
    {
        /// <summary>
        /// Order detailed organisation info (report)
        /// </summary>
        Task<OrganisationSummaryResponseDto> OrderOrganisationSummaryAsync(OrganisationOrderRequestCommand request, CancellationToken cancellationToken);

        /// <summary>
        /// Person lookup order
        /// </summary>
        Task<PersonListResponseDto> OrderPersonSummaryAsync(PersonOrderRequestCommand request, CancellationToken cancellationToken);

        /// <summary>
        /// Person report order
        /// </summary>
        Task<List<OrderDto>> OrderPersonReportsAsync(PersonOrderReportCommand request, CancellationToken cancellationToken);
    }
}
