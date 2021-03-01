using InfoTrack.Cdd.Infrastructure.Utils;
using InfoTrack.Parties.Api.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InfoTrack.Cdd.Api.Attributes
{
    /// <summary>
    /// The user must have certain permissions enabled (i.e. must not have RetailerType == Reseller)
    /// </summary>
    public sealed class UserPermissionRequirementAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// The user must have certain permissions enabled (i.e. must not have RetailerType == Reseller)
        /// </summary>
        public UserPermissionRequirementAttribute() : base(typeof(UserPermissionRequirementFilter))
        {
        }
    }

    /// <summary>
    /// The user must have certain permissions enabled (i.e. must not have RetailerType == Reseller)
    /// </summary>
    public class UserPermissionRequirementFilter : IAuthorizationFilter
    {
        private readonly IUserRequestContext _userRequestContext;

        /// <summary>
        /// The user must have certain permissions enabled (i.e. must not have RetailerType == Reseller)
        /// </summary>
        public UserPermissionRequirementFilter(IUserRequestContext userRequestContext)
        {
            _userRequestContext = userRequestContext;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                // This condition is obsolete (Resellers are now permitted) but this code is left here as an example
                // should other permission restrictions be required in the future
                //if (_userRequestContext.RetailerType == null || _userRequestContext.RetailerType == RetailerTypeEnum.Reseller)
                //{
                //    // Returns a 403 Forbidden response
                //    context.Result = new ForbidResult();
                //}
            }
        }
    }

}
