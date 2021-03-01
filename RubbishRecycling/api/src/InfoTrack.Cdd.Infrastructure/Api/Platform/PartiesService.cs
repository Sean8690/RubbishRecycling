using System;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Infrastructure.Utils;
using InfoTrack.Parties.Api.Client;
using Microsoft.Extensions.Logging;

namespace InfoTrack.Cdd.Infrastructure.Api.Platform
{
    /// <summary>
    /// Interrogate Platform.Core PartiesApi
    /// </summary>
    public class PartiesService : PlatformServiceBase, IPartiesService
    {
        private readonly IPartiesApiClient _client;

        public PartiesService(IPartiesApiClient client, ILogger<PartiesService> logger) : base(logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<RetailerTypeEnum?> GetRetailerTypeAsync(int retailerId, CancellationToken cancellationToken)
        {
            if (retailerId <= 0)
            {
                return null;
            }
            return (await CallAuthority(retailerId, _client.Retailers_GetAsync, cancellationToken)).RetailerType;
        }

    }
}
