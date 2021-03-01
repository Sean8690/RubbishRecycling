using System;
using System.Net.Http;
using Api.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Client
{
    public partial class ApiClient
    {
        [ActivatorUtilitiesConstructor]
        public ApiClient(ApiOptions config, HttpClient httpClient) : this(httpClient)
        {
            // Ensure that there is a trailing slash to make path combinations work later and prevent virtual directories being stripped on requests.
            _httpClient.BaseAddress = new Uri(config.BaseUri.AbsoluteUri.TrimEnd('/') + "/");
        }
    }
}
