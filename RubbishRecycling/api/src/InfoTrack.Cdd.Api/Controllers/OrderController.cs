using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Cdd.Application.Queries.Orders;
using InfoTrack.Common.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InfoTrack.Cdd.Api.Controllers
{
    /// <summary>
    /// Orders
    /// </summary>
    public class OrderController : CddControllerBase
    {
        /// <summary>
        /// Get a list of orders matching the given criteria
        /// </summary>
        /// <remarks>
        /// This can be used for reorder detection
        /// </remarks>
        /// <param name="serviceIdentifier">Service identifier (identifies which report should be ordered)</param>
        /// <param name="providerEntityCode">Provider's unique organisation identifier (current provider is Frankie Financial)</param>
        /// <param name="kyckrCountryCode">Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)</param>
        /// <param name="clientReference">Client reference (aka "matter")</param>
        /// <param name="retailerReference">Retailer reference</param>
        /// <param name="cancellation"></param>
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get(
            [FromQuery, BindRequired, EnumDataType(typeof(ServiceIdentifier)), JsonConverter(typeof(StringEnumConverter))] ServiceIdentifier serviceIdentifier,
            [FromQuery, BindRequired] string providerEntityCode,
            [FromQuery, BindRequired] string kyckrCountryCode,
            [FromQuery, BindRequired, MaxLength(50)] string clientReference,
            [FromQuery, BindRequired] string retailerReference,
            CancellationToken cancellation)
        {
            return Ok(await Mediator.Send(new GetOrdersQuery(serviceIdentifier, providerEntityCode, kyckrCountryCode, clientReference, retailerReference), cancellation));
        }
    }
}
