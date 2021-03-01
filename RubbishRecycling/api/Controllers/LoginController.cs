using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubbishRecyclingAU.ControllerModels;
using RubbishRecyclingAU.ControllerModels.Authentication;
using static RubbishRecyclingAU.ControllerModels.RequestAccess;

namespace RubbishRecyclingAU.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly RubbishRecyclingContext _ef;
        private readonly IAuthenticationWrapper _wrapper;
        private readonly IAntiforgeryService _antiforgery;

        public LoginController(RubbishRecyclingContext rubbishRecyclingContext, IAuthenticationWrapper wrapper, IAntiforgeryService antiforgery)
        {
            _ef = rubbishRecyclingContext;
            _wrapper = wrapper;
            _antiforgery = antiforgery;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        public async Task<AuthenticationResponse> LogIn([FromBody] UserLoginRequest request)
        {
            var user = await _ef.Users.SingleOrDefaultAsync(u => u.Email == request.EmailId)
            ?? throw new Exception("User not found");

            var privilegeLevel = (user.Password == request.Password) ? SessionPrivilegeLevel.Privileged : SessionPrivilegeLevel.Underprivileged;
            var response = await EstablishUserSessionAsync(user.Email, privilegeLevel);
            return response;
        }

        private async Task<AuthenticationResponse> EstablishUserSessionAsync(string emailId, SessionPrivilegeLevel privilegeLevel)
        {
            var response = new AuthenticationResponse
            {
                PrivilegeLevel = privilegeLevel
            };

            var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(AuthenticationConstants.EmailIdClaimType, emailId),
                new Claim(AuthenticationConstants.PrivilegeLevelClaimType, privilegeLevel.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme));
            await _wrapper.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims);
            response.AntiforgeryHeaderValue = _antiforgery.Establish();

            return response;
        }
    }
}