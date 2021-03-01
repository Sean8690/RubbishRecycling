using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using InfoTrack.Cdd.Application.Common.Enums;
using Xunit;

#pragma warning disable CA2234 // Pass system uri objects instead of strings

namespace InfoTrack.Cdd.Api.IntegrationTests.Controllers
{
    public class OrderControllerTests : ControllerTestBase
    {
        public OrderControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [DevFact] 
        public async Task GivenValidQueryString_WhenGetCalled_Then200Response()
        {
            var providerEntityCode = "050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED";
            var kyckrCountryCode = "AU";
            var clientReference = "cdd-integration";
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/Order?serviceIdentifier={ServiceIdentifier.CddOrganisationReport}&providerEntityCode={providerEntityCode}&kyckrCountryCode={kyckrCountryCode}&clientReference={clientReference}");

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(ServiceIdentifier.Undefined, "050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "AU", "cdd-integration")]
        [InlineData(ServiceIdentifier.CddOrganisationReport, null, "AU", "cdd-integration")]
        [InlineData(ServiceIdentifier.CddOrganisationReport, "", "AU", "cdd-integration")]
        [InlineData(ServiceIdentifier.CddOrganisationReport, "050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "AUS", "cdd-integration")]
        [InlineData(ServiceIdentifier.CddOrganisationReport, "050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "", "cdd-integration")]
        [InlineData(ServiceIdentifier.CddOrganisationReport, "050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", null, "cdd-integration")]
        [InlineData(ServiceIdentifier.CddOrganisationReport, "050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "AU", "thisreferenceisover50charslongthisreferenceisover50charslong")]
        [InlineData(ServiceIdentifier.CddOrganisationReport, "050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "AU", null)]
        [InlineData(ServiceIdentifier.CddOrganisationReport, "050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "AU", "")]
        public async Task GivenInvalidQueryArgument_WhenGetCalled_Then400Response(ServiceIdentifier serviceIdentifier, string providerEntityCode, string kyckrCountryCode, string clientReference)
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/Order?serviceIdentifier={serviceIdentifier}&providerEntityCode={providerEntityCode}&kyckrCountryCode={kyckrCountryCode}&clientReference={clientReference}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
