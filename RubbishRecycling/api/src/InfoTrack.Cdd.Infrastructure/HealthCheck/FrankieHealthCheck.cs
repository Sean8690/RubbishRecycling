using System;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InfoTrack.Cdd.Infrastructure.HealthCheck
{
    public class FrankieHealthCheck : IHealthCheck
    {
        private readonly IOrganisationService _organisationService;

        public FrankieHealthCheck(IOrganisationService organisationService)
        {
            _organisationService = organisationService ?? throw new ArgumentNullException(nameof(organisationService));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await _organisationService.PingAsync();
            
            if (!result)
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, "Could not contact Frankie API");
            }

            return new HealthCheckResult(HealthStatus.Healthy);
        }
    }
}
