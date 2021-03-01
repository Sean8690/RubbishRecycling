using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RubbishRecyclingAU.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpGet("Test")]
        public async Task<ActionResult<string>> Test()
        {
            return Ok("Success");
        }


    }
}