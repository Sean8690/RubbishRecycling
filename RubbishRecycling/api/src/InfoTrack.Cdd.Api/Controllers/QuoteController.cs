using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Commands.Quote;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InfoTrack.Cdd.Api.Controllers
{
    /// <summary>
    /// Fees
    /// </summary>
    public class QuoteController : CddControllerBase
    {
        /// <summary>
        /// Get a fee quote for a ServiceIdentifier and country
        /// </summary>
        [ProducesResponseType(typeof(QuoteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<QuoteDto>> Fee(
            [FromQuery, BindRequired, EnumDataType(typeof(ServiceIdentifier)), JsonConverter(typeof(StringEnumConverter))] ServiceIdentifier serviceIdentifier,
            [FromQuery] string kyckrCountryCode,
            CancellationToken cancellation)
        {
            return Ok(await Mediator.Send(new QuoteCommand(serviceIdentifier, kyckrCountryCode), cancellation));
        }
    }
}
