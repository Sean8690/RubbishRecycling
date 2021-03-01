using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using InfoTrack.Cdd.Application.Common.Enums;
using Microsoft.AspNetCore.Http;
using Xunit;

#pragma warning disable CA2234 // Pass system uri objects instead of strings

namespace InfoTrack.Cdd.Api.IntegrationTests.Controllers
{
    public class QuoteControllerTests : ControllerTestBase
    {
        public QuoteControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [DevTheory]
        [InlineData(ServiceIdentifier.CddOrganisationReport, "AU")]
        public async Task GivenValidQueryString_WhenGetCalled_Then200Response(ServiceIdentifier serviceIdentifier, string kyckrCountryCode)
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/quote?serviceIdentifier={serviceIdentifier}&kyckrCountryCode={kyckrCountryCode}");

            response.EnsureSuccessStatusCode();
        }

        [DevTheory]
        [InlineData(ServiceIdentifier.Undefined, "AU")]
        [InlineData(ServiceIdentifier.CddOrganisationReport, "AUS")]
        [InlineData(ServiceIdentifier.CddOrganisationReport, null)]
        // kyckrCountryCode is not longer mandatory since Quote API is now generic to be reused for PersonSearch.
        public async Task GivenInvalidQueryArgument_WhenGetCalled_Then400Response(ServiceIdentifier serviceIdentifier, string kyckrCountryCode)
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/quote?serviceIdentifier={serviceIdentifier}&kyckrCountryCode={kyckrCountryCode}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenMissingServiceIdentifierArgument_WhenGetCalled_Then400Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/quote?kyckrCountryCode=AU");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [DevFact]
        // kyckrCountryCode is not longer mandatory since Quote API is now generic to be reused for PersonSearch.
        public async Task GivenMissingKyckrCountryCodeArgument_WhenGetCalled_Then400Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/quote?serviceIdentifier={ServiceIdentifier.CddOrganisationReport}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
