using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using InfoTrack.Cdd.Application.Dtos;
using Xunit;

#pragma warning disable CA2234 // Pass system uri objects instead of strings

namespace InfoTrack.Cdd.Api.IntegrationTests.Controllers
{
    public class CountryControllerTests : ControllerTestBase
    {
        private const int CountryCount = 173;

        public CountryControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [DevFact]
        public async Task GivenValidQueryString_WhenGetCalled_Then200Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync("/country");

            response.EnsureSuccessStatusCode();
            
            var content = await IntegrationTestHelper.GetResponseContent<IEnumerable<CountryDto>>(response);
            content.Should().NotBeNullOrEmpty();
            content.Should().HaveCount(CountryCount);
        }
    }
}
