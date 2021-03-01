using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Api.Examples;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Cdd.Application.Queries.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace InfoTrack.Cdd.Api.Controllers
{
    /// <summary>
    /// Countries
    /// </summary>
    public class CountryController : CddControllerBase
    {
        /// <summary>
        /// Get a list of supported countries
        /// </summary>
        /// <remarks>
        /// Returns a full list of supported countries
        /// </remarks>
        [ProducesResponseType(typeof(IEnumerable<CountryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(IEnumerable<CountryDto>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CountryDtoListExample))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetSupportedCountries(CancellationToken cancellation)
        {
            return Ok(await Mediator.Send(new GetCountriesQuery(), cancellation));
        }
    }
}
