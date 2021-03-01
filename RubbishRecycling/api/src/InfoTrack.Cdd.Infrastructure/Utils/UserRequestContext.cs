using System;
using System.Threading;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Net.Http;
using InfoTrack.Parties.Api.Client;
using InfoTrack.Web.Base.Extensions;
using Microsoft.AspNetCore.Http;

namespace InfoTrack.Cdd.Infrastructure.Utils
{
    public interface IUserRequestContext
    {
        string AccessToken { get; }
        int LoginId { get; }
        int ClientId { get; }
        int RetailerId { get; }
        RetailerTypeEnum? RetailerType { get; }
    }

    public class UserRequestContext : IUserRequestContext
    {
        private const string AuthorizationHeader = "Authorization";

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IPartiesService _partiesService;

        public string AccessToken { get; private set; }
        public int LoginId { get; private set; }
        public int ClientId { get; private set; }
        public int RetailerId { get; private set; }
        public RetailerTypeEnum? RetailerType { get; private set; }

        public UserRequestContext(IHttpContextAccessor httpContextAccessor, IPartiesService partiesService)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _partiesService = partiesService ?? throw new ArgumentNullException(nameof(partiesService));
            SetValues();
        }

        private void SetValues()
        {
            AccessToken = GetAuthorizationToken(_httpContextAccessor.HttpContext);
            if (_httpContextAccessor.HttpContext != null)
            {
                LoginId = _httpContextAccessor.HttpContext.User.Identity.GetLoginId();
                ClientId = _httpContextAccessor.HttpContext.User.Identity.GetClientId();
                RetailerId = _httpContextAccessor.HttpContext.User.Identity.GetRetailerId();
                // TODO cache this so we don't have to keep looking it up
                RetailerType = _partiesService.GetRetailerTypeAsync(RetailerId, new CancellationToken()).GetAwaiter().GetResult();
            }
        }

        private string GetAuthorizationToken(HttpContext httpContext)
        {
            string token = null;
            if (httpContext != null && httpContext.Request.Headers.ContainsKey(AuthorizationHeader))
            {
                var header = new HttpHeader(AuthorizationHeader, httpContext.Request.Headers[AuthorizationHeader]);
                token = header.Value.Substring("Bearer ".Length);
            }
            return token;
        }
    }
}
