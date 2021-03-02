using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.ControllerModels.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmailId(this ClaimsPrincipal user) => user.GetClaimValue(
            AuthenticationConstants.EmailIdClaimType);

        public static RequestAccess.SessionPrivilegeLevel? GetPrivilegeLevel(this ClaimsPrincipal user) =>
            Enum.TryParse<RequestAccess.SessionPrivilegeLevel>(user.GetClaimValue(AuthenticationConstants.PrivilegeLevelClaimType), out var privilegeLevel)
                ? privilegeLevel
                : (RequestAccess.SessionPrivilegeLevel?)null;

        private static string GetClaimValue(this ClaimsPrincipal user, string type) =>
            user.FindFirst(c => c.Type == type)?.Value;
    }
}
