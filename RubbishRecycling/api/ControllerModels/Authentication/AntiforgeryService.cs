using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.ControllerModels.Authentication
{
    public interface IAntiforgeryService
    {
        string Establish();
        string EstablishApi();
        bool Validate();
        bool ValidateApi();
    }

    public class AntiforgeryService : IAntiforgeryService
    {
        private readonly IAuthenticationWrapper _wrapper;

        public AntiforgeryService(IAuthenticationWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public string Establish() => Establish(AuthenticationConstants.AntiforgeryCookieName, AuthenticationConstants.AntiforgeryCookieLifetime);
        public string EstablishApi() => Establish(AuthenticationConstants.ApiAntiforgeryCookieName, AuthenticationConstants.AntiforgeryCookieLifetime);

        private string Establish(string cookieName, TimeSpan lifetime)
        {
            var token = Guid.NewGuid().ToString();
            _wrapper.AddCookie(cookieName, token, new CookieOptions { MaxAge = lifetime });
            return token;
        }

        public bool Validate() => Validate(AuthenticationConstants.AntiforgeryHeaderName, AuthenticationConstants.AntiforgeryCookieName);
        public bool ValidateApi() => Validate(AuthenticationConstants.ApiAntiforgeryHeaderName, AuthenticationConstants.ApiAntiforgeryCookieName);

        private bool Validate(string headerName, string cookieName)
        {
            var headerValue = _wrapper.GetHeader(headerName);
            var cookieValue = _wrapper.GetCookie(cookieName);
            if (string.IsNullOrEmpty(headerValue) || string.IsNullOrEmpty(cookieValue))
            {
                return false;
            }
            return headerValue == cookieValue;
        }
    }
}
