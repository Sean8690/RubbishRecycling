using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Commands.PersonOrder;
using InfoTrack.Cdd.Application.Dtos.Order;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    public interface IPersonLookUpServiceFacade
    {
        /// <summary>
        /// Order detailed person info (report)
        /// </summary>
        Task<OrganisationSummaryResponseDto> OrderOrganisationSummaryAsync(PersonOrderRequestCommand request, CancellationToken cancellationToken);
    }
}
