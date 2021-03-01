using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.ControllerModels.Authentication
{
    public class AuthenticationConstants
    {
        public const string EmailIdClaimType = "Email";
        public const string PrivilegeLevelClaimType = "PrivilegeLevel";
        public const string ApiConsumerIdClaimType = "ApiConsumer";

        public const int BCryptWorkFactor = 12;

        public const string MfaCookieNamePrefix = "Mfa_";
        public const string AuthenticationCookieName = "Auth";

        public const string ApiAuthorisationHeaderName = "X-API-Authorization";

        public const string AntiforgeryCookieName = "Xsrf";
        public const string AntiforgeryHeaderName = "X-Xsrf";

        public const string ApiAntiforgeryCookieName = "Xsrfapi";
        public const string ApiAntiforgeryHeaderName = "X-Xsrf-API";

        // The front end guarantees to send a request no less frequently than every 5
        // minutes (while the window remains open, obviously).
        public static TimeSpan AuthenticationCookieLifetime = TimeSpan.FromMinutes(30);

        public static TimeSpan MfaCookieLifetime = TimeSpan.FromDays(365);

        // This should be longer than MfaCookieLifetime.
        public static TimeSpan AntiforgeryCookieLifetime = TimeSpan.FromDays(366);
    }
}
