using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RubbishRecyclingAU.ControllerModels;
using RubbishRecyclingAU.Services;

namespace RubbishRecyclingAU.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IRegisterService _registerService;

        public UserController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        //Register
        [HttpPost("register")]
        public async Task<ActionResult<int>> Register([FromBody] UserDetails userDetails)
        {
            var result = await _registerService.RegisterUser(userDetails);
            if (!result.CanProceed)
            {
                return BadRequest(new { result.Message });
            }
            return Ok(result);
        }


    }
}