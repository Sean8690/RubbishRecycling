using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;

namespace InfoTrack.Cdd.Api.IntegrationTests.Mocks
{
    public class TestServicesService : IServicesService
    {
        public async Task<int> GetServiceIdentifier(ServiceIdentifier serviceIdentifier, CancellationToken cancellationToken)
        {
            return await Task.FromResult(12345);
        }
    }
}
