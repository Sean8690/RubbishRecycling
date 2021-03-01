using InfoTrack.Common.Api.Client;
using Api.Client.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Client.DependencyInjection
{
    public static class ApiClientBuilderExtensions
    {
        public static IApiClientBuilder AddHttpAuthentication(this IApiClientBuilder builder)
        {
            builder.Services.AddTransient<HttpAuthenticationHandler>();

            builder.HttpClientBuilder
                .AddHttpMessageHandler<HttpAuthenticationHandler>();

            return builder;
        }

        public static IApiClientBuilder AddMachineAuthentication(this IApiClientBuilder builder)
        {
            builder.Services.AddTransient<NonHttpAuthenticationHandler>();

            builder.HttpClientBuilder
                .AddHttpMessageHandler<NonHttpAuthenticationHandler>();

            return builder;
        }
    }
}
