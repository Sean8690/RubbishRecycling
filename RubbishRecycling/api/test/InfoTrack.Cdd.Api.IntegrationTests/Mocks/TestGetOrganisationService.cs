using System.Collections.Generic;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Api.IntegrationTests.Mocks
{
    public class TestGetOrganisationService : IOrganisationService
    {
        public async Task<bool> PingAsync() => await Task.FromResult(true);

        public async Task<IEnumerable<OrganisationLite>> GetOrganisationsAsync(string name, string number, string kyckrCountryCode)
        {
            return await Task.FromResult(new List<OrganisationLite>
            {
                new OrganisationLite
                {
                    Name = name, 
                    Number = number,
                    KyckrCountryCode = kyckrCountryCode
                }
            } as IEnumerable<OrganisationLite>);
        }

        public async Task<Organisation> GetOrganisationAsync(string providerEntityCode, string kyckrCountryCode, int orderId, ServiceIdentifier serviceIdentifier)
        {
            return await Task.FromResult(
                new Organisation
                {
                    RawResponse = "integration test raw response placeholder",
                    //Reports = new Dictionary<ServiceIdentifier, FileMetadata>(),
                    Name = "Integration Test"
                });
        }
    }
}
