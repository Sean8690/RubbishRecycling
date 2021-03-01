using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Api.Controllers;
using InfoTrack.Cdd.Application.Commands.PersonOrder;
using InfoTrack.Cdd.Application.Commands.PersonReport;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Cdd.Application.Dtos.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.CreditorWatch.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class PersonLookupController : CddControllerBase
    {
        /// <summary>
        /// Order an AML report for a person
        /// </summary>
        /// <returns>Returns details about the order created.</returns>
        /// <param name="request">Look Up Request</param>
        /// <param name="cancellation">Cancellation token</param>
        [ProducesResponseType(typeof(PersonListResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("order")]
        public async Task<ActionResult<PersonListResponseDto>> Post([FromBody] AmlPersonLookupRequest request,
            CancellationToken cancellation)
        {
            return Ok(await Mediator.Send(new PersonOrderRequestCommand(request), cancellation));
        }

        /// <summary>
        /// Order a report for selected person
        /// </summary>
        /// <returns>Returns details about the order created.</returns>
        /// <param name="reportRequest"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("order/report")]
        public async Task<ActionResult<OrderDto>> GenerateReport([FromBody] AmlPersonLookupReportRequest reportRequest,
            CancellationToken cancellation)
        {
            return Ok(await Mediator.Send(new PersonOrderReportCommand(
                    reportRequest?.OrderId ?? 0,
                    reportRequest?.ProviderEntityCodes,
                    ServiceIdentifier.CddPersonRiskReport,
                    quoteId: null),
                cancellation));
        }
    }
}
