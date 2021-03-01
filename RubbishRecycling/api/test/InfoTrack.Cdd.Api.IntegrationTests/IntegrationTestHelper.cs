using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InfoTrack.Cdd.Api.IntegrationTests
{
    public static class IntegrationTestHelper
    {
        public static IConfiguration GetTestConfiguration()
        => new ConfigurationBuilder()
            .AddJsonFile("integrationsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .AddEnvironmentVariables()
            .Build();

        public static StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        public static async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<T>(stringResponse);

            return result;
        }
    }
}
