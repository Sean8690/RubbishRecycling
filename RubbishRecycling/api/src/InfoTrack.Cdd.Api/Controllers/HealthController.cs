using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using InfoTrack.Common.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// (This is not necessary, it's just nice-to-have for all endpoints to appear in the swagger documentation. If this causes any unexpected problems, it can just be removed.)

namespace InfoTrack.Cdd.Api.Controllers
{
    /// <summary>
    /// Health checks
    /// </summary>
    public class HealthController : ApiController
    {
        /// <summary>
        /// Run a health check
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.ServiceUnavailable)]
        [Produces(MediaTypeNames.Text.Plain)] // "text/plain"
        public async Task<string> Get(
            [FromServices] HealthCheckService healthCheckService,
            [FromServices] HealthCheckOptions healthCheckOptions
        )
        {
            if (healthCheckService == null)
            {
                throw new ArgumentNullException(nameof(healthCheckService));
            }
            if (healthCheckOptions == null)
            {
                throw new ArgumentNullException(nameof(healthCheckOptions));
            }

            HealthReport report = await healthCheckService.CheckHealthAsync(healthCheckOptions.Predicate, HttpContext.RequestAborted);
            Response.StatusCode = healthCheckOptions.ResultStatusCodes[report.Status];
            return report.Status.ToString();
        }
    }
}
