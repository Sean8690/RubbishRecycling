using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.ControllerModels.Authentication
{
    public interface IAuthenticationWrapper
    {
        Task SignInAsync(string scheme, ClaimsPrincipal principal);
        Task SignOutAsync(string scheme);
        void AddCookie(string name, string value, CookieOptions options);
        void AddHeader(string name, string value);
        string GetCookie(string name);
        string GetHeader(string name);
    }

    public class AuthenticationWrapper : IAuthenticationWrapper
    {
        // Unfortunately this can't be injected because there is no 'HttpContext.Current' in .NET
        // Core. Also IHttpContextAccessor doesn't work properly. So instead we will set this value
        // from AuthenticationWrapperFilter.
        public HttpContext HttpContext { get; set; }

        public Task SignInAsync(string scheme, ClaimsPrincipal principal) => HttpContext.SignInAsync(scheme, principal);

        public Task SignOutAsync(string scheme) => HttpContext.SignOutAsync(scheme);

        public void AddCookie(string name, string value, CookieOptions options) =>
            HttpContext.Response.Cookies.Append(name, value, options);

        public string GetCookie(string name) =>
            HttpContext.Request.Cookies.TryGetValue(name, out var value) ? value : null;

        public string GetHeader(string name) =>
            HttpContext.Request.Headers.TryGetValue(name, out var value) ? value.ToString() : null;

        public void AddHeader(string name, string value) =>
            HttpContext.Response.Headers.Append(name, value);
    }
}
