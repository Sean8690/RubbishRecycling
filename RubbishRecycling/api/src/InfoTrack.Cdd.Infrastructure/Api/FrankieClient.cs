using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Cdd.Infrastructure.Config;
using InfoTrack.Net.Http.Configuration;
using InfoTrack.Net.Http.Configuration.Resiliency;
using Newtonsoft.Json;
using HttpClient = InfoTrack.Net.Http.HttpClient;

namespace InfoTrack.Cdd.Infrastructure.Api
{
    public class FrankieClient : ITestableOrganisationClient, ITestablePersonClient
    {
        private readonly FrankieApiConfig _frankieConfig;
        private readonly HttpClient _client;

        private const string PingEndpoint = "ruok";
        private const string OrganisationLookupEndpoint = "business/international/search";
        private const string OrganisationProfileEndpoint = "business/international/profile";
        private const string PersonLookupEndpoint = "entity/new/verify/pep_media/full";

        public FrankieClient(FrankieApiConfig config)
        {
            _frankieConfig = config ?? throw new ArgumentNullException(nameof(config));
            _client = new HttpClient(
                _frankieConfig.BaseUri.ToString(),
                new HttpClientOptions
                {
                    RetryPolicy = Retry
                        .OnHttpStatus((int)HttpStatusCode.ServiceUnavailable) // 503 "Authority Unavailable" is remarkably common
                        .WithExponentialBackoff()
                        .StopAfter(5),
                    RequestHeaders = new Dictionary<string, string>
                    {
                        { "X-Frankie-CustomerID", config.CustomerId },
                        { "api_key", config.ApiKey }
                    }
                });
        }

        public async Task<TestableClientResponse> PingAsync()
        {
            var response = await _client.GetAsync(PingEndpoint);
            return new TestableClientResponse
            {
                ResponseMessage = response,
                ResponseString = await response.Content.ReadAsStringAsync()
            };
        }

        public async Task<TestableClientResponse> GetOrganisationsAsync<TRequest>(TRequest request)
            => await PostAsync(request, OrganisationLookupEndpoint);

        public async Task<TestableClientResponse> GetOrganisationAsync<TRequest>(TRequest request)
            => await PostAsync(request, OrganisationProfileEndpoint);

        public async Task<TestableClientResponse> GetPersonsAsync<TRequest>(TRequest request)
            => await PostAsync(request, PersonLookupEndpoint);

        private async Task<TestableClientResponse> PostAsync<TRequest>(TRequest request, string endpoint)
        {
            var req = JsonConvert.SerializeObject(request, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            using var httpContent = new StringContent(req, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(endpoint, httpContent);

            return new TestableClientResponse
            {
                ResponseMessage = response,
                ResponseString = await response.Content.ReadAsStringAsync()
            };
        }

    }
}
