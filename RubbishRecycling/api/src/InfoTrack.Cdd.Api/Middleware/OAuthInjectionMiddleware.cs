using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace InfoTrack.Cdd.Api.Middleware
{
    
    public class OAuthInjectionMiddleware
    {
        private readonly RequestDelegate _next;

        public OAuthInjectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string token;
            context.Request.Cookies.TryGetValue("access_token", out token);
            
            if (!string.IsNullOrEmpty(token))
            {
                if (context.Request.Headers.Keys.Contains("Authorization"))
                {
                    context.Request.Headers.Remove("Authorization");
                    context.Response.Headers.Remove("Authorization");
                }
                context.Request.Headers.Add("Authorization", new StringValues("Bearer " + token));
            }
            await _next.Invoke(context);
        }
    }

    public static class OAuthInjectionExtension
    {
        public static IApplicationBuilder ApplyOAuthInjection(this IApplicationBuilder app)
        {
            app.UseMiddleware<OAuthInjectionMiddleware>();
            return app;
        }
    }
    
}
