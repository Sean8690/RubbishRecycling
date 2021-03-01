using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using InfoTrack.Cdd.Application.Common.Enums;
using Xunit;

#pragma warning disable CA2234 // Pass system uri objects instead of strings

namespace InfoTrack.Cdd.Api.IntegrationTests.Controllers
{
    public class OrganisationControllerTests : ControllerTestBase
    {
        public OrganisationControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [DevTheory]
        [InlineData("frankie", "AU")]
        public async Task GivenValidQueryString_WhenGetCalled_Then200Response(string name, string kyckrCountryCode)
        {
            
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/organisation?name={name}&kyckrCountryCode={kyckrCountryCode}");

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("frankie", "AUS")]
        [InlineData("frankie", "")]
        [InlineData("frankie", null)]
        [InlineData(null, "AU")]
        public async Task GivenInvalidQueryArgument_WhenGetCalled_Then400Response(string name, string kyckrCountryCode)
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/organisation?name={name}&kyckrCountryCode={kyckrCountryCode}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenMissingKyckrCountryCodeArgument_WhenGetCalled_Then400Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/organisation?name=frankie");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenMissingNameAndNumberArguments_WhenGetCalled_Then400Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync($"/organisation?kyckrCountryCode=AU");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [DevTheory]
        [InlineData("050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "AU", ServiceIdentifier.CddOrganisationReport)]
        public async Task GivenValidQueryString_WhenPostCalled_Then200Response(string providerEntityCode, string kyckrCountryCode, ServiceIdentifier serviceIdentifier)
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.PostAsync($"/organisation/{providerEntityCode}/order?kyckrCountryCode={kyckrCountryCode}&serviceIdentifier={serviceIdentifier}&clientReference=cdd-integration", null);

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "AU", ServiceIdentifier.Undefined)]
        [InlineData("050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "AUS", ServiceIdentifier.CddOrganisationReport)]
        [InlineData("050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", "", ServiceIdentifier.CddOrganisationReport)]
        [InlineData("050308962_FRANKIE GOES TO HOLLYWOOD INCORPORATED", null, ServiceIdentifier.CddOrganisationReport)]
        public async Task GivenInvalidQueryArgument_WhenPostCalled_Then400Response(string providerEntityCode, string kyckrCountryCode, ServiceIdentifier serviceIdentifier)
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.PostAsync($"/organisation/{providerEntityCode}/order?kyckrCountryCode={kyckrCountryCode}&serviceIdentifier={serviceIdentifier}&clientReference=cdd-integration", null);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("", "AU", ServiceIdentifier.CddOrganisationReport)]
        [InlineData(null, "AU", ServiceIdentifier.CddOrganisationReport)]
        public async Task GivenInvalidRouteArgument_WhenPostCalled_Then404Response(string providerEntityCode, string kyckrCountryCode, ServiceIdentifier serviceIdentifier)
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.PostAsync($"/organisation/{providerEntityCode}/order?kyckrCountryCode={kyckrCountryCode}&serviceIdentifier={serviceIdentifier}&clientReference=cdd-integration", null);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenMissingKyckrCountryCodeArgument_WhenPostCalled_Then400Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.PostAsync($"/organisation/providerEntityCode/order?serviceIdentifier={ServiceIdentifier.CddOrganisationReport}&clientReference=cdd-integration", null);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenMissingServiceIdentifierArgument_WhenPostCalled_Then400Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.PostAsync($"/organisation/providerEntityCode/order?kyckrCountryCode=AU&clientReference=cdd-integration", null);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenMissingClientReferenceArgument_WhenPostCalled_Then400Response()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.PostAsync($"/organisation/providerEntityCode/order?serviceIdentifier={ServiceIdentifier.CddOrganisationReport}&kyckrCountryCode=AU", null);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
