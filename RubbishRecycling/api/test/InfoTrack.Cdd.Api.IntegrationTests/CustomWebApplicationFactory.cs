using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InfoTrack.Cdd.Api.IntegrationTests.Mocks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfoTrack.Cdd.Api.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private static readonly HttpClient AuthClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("integrationsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .AddEnvironmentVariables()
                .Build();

                config.AddConfiguration(integrationConfig);
            })
            .ConfigureServices(services =>
            {
                // Register test services
                AddTestServices(services);
            })
            .UseEnvironment("Development");
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            return await GetAuthenticatedClientAsync(
                IntegrationTestHelper.GetTestConfiguration().GetSection("IntegrationAuthOptions")["Username"],
                IntegrationTestHelper.GetTestConfiguration().GetSection("IntegrationAuthOptions")["Password"]
            );
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync(string userName, string password)
        {
            var client = CreateClient();

            var token = await GetAccessTokenAsync(userName, password);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        private static async Task<string> GetAccessTokenAsync(string userName, string password)
        {
            var authToken = $"{IntegrationTestHelper.GetTestConfiguration().GetSection("IntegrationAuthOptions")["ClientId"]}:" +
                $"{IntegrationTestHelper.GetTestConfiguration().GetSection("IntegrationAuthOptions")["ClientSecret"]}";
            var authTokenEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(authToken));
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                IntegrationTestHelper.GetTestConfiguration().GetSection("IntegrationAuthOptions")["Authority"].TrimEnd('/') + "/connect/token"
                );
            req.Headers.Authorization = new AuthenticationHeaderValue("Basic", authTokenEncoded);

            var payload = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", userName },
                { "password", password },
                { "scope", "openid offline_access profile infsec:identity infsec:basic" },
            };

            req.Content = new FormUrlEncodedContent(payload);
            req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var httpResponse = await AuthClient.SendAsync(req);

            if (!httpResponse.IsSuccessStatusCode)
            {
                #pragma warning disable CA2201 // Do not raise reserved exception types
                throw new Exception(httpResponse.ReasonPhrase);
                #pragma warning restore CA2201 // Do not raise reserved exception types
            }

            var data = await httpResponse.Content.ReadAsStringAsync();
            var jsonResp = JsonSerializer.Deserialize<JsonElement>(data);
            return jsonResp.GetProperty("access_token").GetString();
        }

        private static void AddTestServices(IServiceCollection services)
        {
            // Local testing hint: any/all the of the mock services below can be commented out
            // if you want to execute integration tests against live endpoints instead.
            // This is very useful for testing (e.g. the full order workflow can be triggered
            // without running or logging into the UI)

            //Configure test services here.
            services.AddTransient<IOrganisationService, TestGetOrganisationService>();
            services.AddTransient<ITestableOrganisationClient, TestOrganisationClient>(s =>
                new TestOrganisationClient(
                    "InternationalBusinessSearchResponse/LogosAustralia_SG",
                    "InternationalBusinessProfileResponse/LogosAustralia_SG"));

            // TODO consider whether the OrderService should not be mocked (if we don't mock, actual
            // orders are created when automation tests are executed.
            // Currently, running against real OrderService works locally but the mock is required on TC.
            services.AddTransient<IOrderService, TestOrderService>();
            services.AddTransient<IQuoteService, TestQuoteService>();
            services.AddTransient<IServicesService, TestServicesService>();
        }
    }
}
