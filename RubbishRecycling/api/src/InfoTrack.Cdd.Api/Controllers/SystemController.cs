using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Cdd.Application.Queries.SystemInfo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.Cdd.Api.Controllers
{
    /// <summary>
    /// System info
    /// </summary>
    public class SystemController : CddControllerBase
    {
        /// <summary>
        /// Get system and region info
        /// </summary>
        /// <remarks>
        /// Returns information about the system and region. This can be used to confirm that deployments are configured correctly and are up to date.
        /// <br /><br />
        /// Additional information is logged to Kibana, so check there if the info you need is not on the response.
        /// </remarks>
        [ProducesResponseType(typeof(SystemInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<SystemInfoDto>> GetSystemInfo(CancellationToken cancellation)
        {
            return Ok(await Mediator.Send(new GetSystemInfoQuery(), cancellation));
        }
    }
}
