using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

#pragma warning disable CA2234 // Pass system uri objects instead of strings

namespace InfoTrack.Cdd.Api.IntegrationTests.Controllers
{
    public class HealthControllerTests : ControllerTestBase
    {
        public HealthControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [DevFact]
        public async Task GivenValidQueryString_WhenGetCalled_Then200Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync("/health");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("Healthy");
        }
    }
}
