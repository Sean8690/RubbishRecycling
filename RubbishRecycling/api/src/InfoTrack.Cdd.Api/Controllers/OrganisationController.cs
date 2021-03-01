using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Commands.Order;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Cdd.Application.Queries.Organisations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InfoTrack.Cdd.Api.Controllers
{
    /// <summary>
    /// Organisations
    /// </summary>
    public class OrganisationController : CddControllerBase
    {
        /// <summary>
        /// Lookup an organisation by name and/or number
        /// </summary>
        /// <remarks>
        /// Returns a list of organisations matching the lookup query.
        ///
        /// Name OR number is required (or both).
        /// </remarks>
        /// <param name="name">Organisation name</param>
        /// <param name="number">Organisation number, e.g. ABN or ACN</param>
        /// <param name="kyckrCountryCode">Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)</param>
        /// <param name="cancellation">Cancellation token</param>
        [ProducesResponseType(typeof(IEnumerable<OrganisationLiteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganisationLiteDto>>> Get(
            [FromQuery] string name,
            [FromQuery] string number,
            [FromQuery, BindRequired] string kyckrCountryCode,
            CancellationToken cancellation)
        {
            return Ok(await Mediator.Send(new GetOrganisationsQuery(name, number, kyckrCountryCode), cancellation));
        }

        /// <summary>
        /// Order a report for an organisation
        /// </summary>
        /// <returns>Returns details about the order created.</returns>
        /// <param name="providerEntityCode">Provider's unique organisation identifier (current provider is Frankie Financial)</param>
        /// <param name="kyckrCountryCode">Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)</param>
        /// <param name="serviceIdentifier">Service identifier (identifies which report should be ordered)</param>
        /// <param name="clientReference">Client reference (aka "matter")</param>
        /// <param name="retailerReference">Retailer reference</param>
        /// <param name="quoteId">Quote Id (optional)</param>
        /// <param name="cancellation">Cancellation token</param>
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{providerEntityCode}/order")]
        public async Task<ActionResult<OrderDto>> Post(
            [FromRoute, BindRequired] string providerEntityCode,
            [FromQuery, BindRequired] string kyckrCountryCode,
            [FromQuery, BindRequired, EnumDataType(typeof(ServiceIdentifier)), JsonConverter(typeof(StringEnumConverter))] ServiceIdentifier serviceIdentifier,
            [FromQuery, BindRequired, MaxLength(50)] string clientReference,
            [FromQuery] string retailerReference,
            [FromQuery] Guid? quoteId,
            CancellationToken cancellation)
        {
            return Ok(await Mediator.Send(new OrganisationOrderRequestCommand(providerEntityCode, kyckrCountryCode, serviceIdentifier, clientReference, retailerReference, quoteId), cancellation));
        }
    }
}
