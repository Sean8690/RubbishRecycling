using System;
using Api.Client.Contracts;
using InfoTrack.Cdd.Api.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Client.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IApiClientBuilder AddGlobalApiClient(this IServiceCollection services, Action<ApiOptions> optionsAction)
        {
            var options = new ApiOptions();
            services.AddSingleton(options);
            optionsAction?.Invoke(options);

            var clientBuilder = services
                .AddHttpClient<IApiClient, ApiClient>();

            return new ApiClientBuilder(services, clientBuilder);
        }
    }
}
