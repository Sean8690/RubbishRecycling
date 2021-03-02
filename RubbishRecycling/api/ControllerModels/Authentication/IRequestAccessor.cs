using System.Net;

namespace RubbishRecyclingAU.ControllerModels.Authentication
{
    public interface IRequestAccessor
    {

        string Email { get; set; }


        /// <summary>The IP address of the client (behind pfSense and Rancher load balancer)</summary>
        IPAddress Ip { get; set; }

        /// <summary>The value of the User-Agent header or null. Implementations are encouraged to
        /// truncate long values to some sane length.</summary>
        string UserAgent { get; set; }

        /// <summary>Set by PermissionService when permissions have been checked for the
        /// request.</summary>
        bool PermissionChecked { get; set; }

        /// <summary>The privilege level of the current session</summary>
        RequestAccess.SessionPrivilegeLevel? PrivilegeLevel { get; set; }

        /// <summary>If non-null, a user name supplied via HTTP basic authentication for which the
        /// password matched the configured value</summary>
        string BasicallyAuthenticatedUserName { get; set; }

        bool SameSiteRequest { get; set; }

        bool SameSiteApiRequest { get; set; }

    }

    public class RequestAccessor : IRequestAccessor
    {
        public string Email { get; set; }

        public IPAddress Ip { get; set; }

        public string UserAgent { get; set; }

        public bool PermissionChecked { get; set; }

        public RequestAccess.SessionPrivilegeLevel? PrivilegeLevel { get; set; }

        public string BasicallyAuthenticatedUserName { get; set; }

        public bool SameSiteRequest { get; set; }

        public bool SameSiteApiRequest { get; set; }
    }
}
