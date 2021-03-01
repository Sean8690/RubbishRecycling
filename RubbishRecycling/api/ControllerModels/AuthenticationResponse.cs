using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RubbishRecyclingAU.ControllerModels.RequestAccess;

namespace RubbishRecyclingAU.ControllerModels
{
    public class SessionPrivilegeLevelResponse
    {
        public SessionPrivilegeLevel? PrivilegeLevel { get; set; }
    }

    public class AuthenticationResponse : SessionPrivilegeLevelResponse
    {
        public string AntiforgeryHeaderValue { get; set; }
        public string ApiAntiforgeryHeaderValue { get; set; }
        public string MfaToken { get; set; }
    }

    public class LoginByTokenResponse : AuthenticationResponse
    {
        public bool Authenticated { get; set; }
        public bool RequireRegistration { get; set; }
        public bool IsLoggedInWithDifferentUser { get; set; }
    }

    public class TokenDetails
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool EmailExistsButNotLinked { get; set; }
        public bool EmailExistsAndLinked { get; set; }
    }

    public class LoginByTokenAuthResponse : AuthenticationResponse
    {
        public bool Authenticated { get; set; }
        public bool SmsCodeValid { get; set; }
    }

    public class LoginByInfotrackAuthResponse : AuthenticationResponse
    {
        public bool Authenticated { get; set; }
        public bool EmailExists { get; set; }
        public bool CanBeLinkedToExistingUser { get; set; }
        public string InfotrackUserEmail { get; set; }
        public string InfortrackUserPhone { get; set; }
        public bool SmsCodeValid { get; set; }
        public string Message { get; set; }
        public DateTime LoginLinkExpireTimeUtc { get; set; }
    }
}
