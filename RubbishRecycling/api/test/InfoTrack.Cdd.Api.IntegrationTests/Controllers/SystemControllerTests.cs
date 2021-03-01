using System.Threading.Tasks;
using Xunit;

#pragma warning disable CA2234 // Pass system uri objects instead of strings

namespace InfoTrack.Cdd.Api.IntegrationTests.Controllers
{
    public class SystemControllerTests : ControllerTestBase
    {
        public SystemControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [DevFact]
        public async Task GivenValidQueryString_WhenGetCalled_Then200Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync("/system");

            response.EnsureSuccessStatusCode();
        }
    }
}
