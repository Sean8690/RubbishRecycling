using Microsoft.Extensions.DependencyInjection;

namespace Api.Client.Contracts
{
    public interface IApiClientBuilder
    {
        IServiceCollection Services { get; }
        IHttpClientBuilder HttpClientBuilder { get; }
    }
}
