using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RubbishRecyclingAU.ControllerModels.Authentication
{
    /// <summary>Inject the current request's HttpContext into the request-scoped
    /// AuthenticationWrapper. This is necessary because there is no longer a global
    /// 'HttpContext.Current' in .NET Core which might have been used from Startup.</summary>
    public class AuthenticationWrapperFilter : IAsyncActionFilter
    {
        private readonly IAuthenticationWrapper _wrapper;

        public AuthenticationWrapperFilter(IAuthenticationWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_wrapper is AuthenticationWrapper concrete)
            {
                concrete.HttpContext = context.HttpContext;
            }

            return next();
        }
    }
}
