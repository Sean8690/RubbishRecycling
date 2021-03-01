using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace RubbishRecyclingAU.ControllerModels.Authentication
{
    public class RequestAccessorFilter : IAsyncActionFilter
    {
        private const int TruncateUserAgentAfterBytes = 4096;
        private readonly IRequestAccessor _accessor;
        private readonly ILogger<RequestAccessorFilter> _logger;
        private readonly IAntiforgeryService _antiforgery;

        public RequestAccessorFilter(
            IRequestAccessor accessor,
            ILogger<RequestAccessorFilter> logger,
            IAntiforgeryService antiforgery)
        {
            _accessor = accessor;
            _logger = logger;
            _antiforgery = antiforgery;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _accessor.SameSiteRequest = _antiforgery.Validate();
            _accessor.SameSiteApiRequest = _antiforgery.ValidateApi();

            if (_accessor.SameSiteRequest || _accessor.SameSiteApiRequest)
            {
                var user = ((ControllerBase)context.Controller).User;

                // Claims from the auth cookie
                if (_accessor.SameSiteRequest)
                {
                    _accessor.Email = user.GetEmailId();
                    _accessor.PrivilegeLevel = user.GetPrivilegeLevel();
                }
            }

            var http = context.HttpContext;

            // This respects X-Forwarded-For:
            // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-2.1
            _accessor.Ip = http.Connection.RemoteIpAddress;

            var userAgent = (string)http.Request.Headers["User-Agent"];
            if (userAgent != null && userAgent.Length > TruncateUserAgentAfterBytes)
            {
                userAgent = userAgent.Substring(0, TruncateUserAgentAfterBytes);
            }
            _accessor.UserAgent = userAgent;
            _accessor.PermissionChecked = true;

            // Execute the action.
            var executedContext = await next();

            // Check the permission check.
            if (_accessor.PermissionChecked || executedContext.Exception != null)
            {
                return;
            }
            _logger.LogError("Missing permission check for action {DisplayName}", executedContext.ActionDescriptor.DisplayName);
            executedContext.Result = new UnauthorizedResult();
        }
    }
}
