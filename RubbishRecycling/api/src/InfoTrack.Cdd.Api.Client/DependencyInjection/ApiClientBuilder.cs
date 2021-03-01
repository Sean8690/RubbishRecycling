using System;
using Api.Client.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Client.DependencyInjection
{
    public class ApiClientBuilder : Contracts.IApiClientBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClientBuilder"/> class.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public ApiClientBuilder(IServiceCollection services, IHttpClientBuilder clientBuilder)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            HttpClientBuilder = clientBuilder ?? throw new ArgumentNullException(nameof(clientBuilder));
        }

        public IServiceCollection Services { get; }
        public IHttpClientBuilder HttpClientBuilder { get; }
    }
}
