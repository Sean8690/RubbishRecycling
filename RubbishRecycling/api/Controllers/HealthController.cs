using InfoTrack.Common.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.Controllers
{
    public class HealthController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.ServiceUnavailable)]
        public async Task<ActionResult<string>> Get()
        {
            return Ok("Healthy");
        }
    }
}