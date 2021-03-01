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
    }
}
