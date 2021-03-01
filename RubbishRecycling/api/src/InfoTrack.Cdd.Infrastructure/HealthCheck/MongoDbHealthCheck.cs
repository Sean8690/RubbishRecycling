using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Infrastructure.Service;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InfoTrack.Cdd.Infrastructure.HealthCheck
{
    public class MongoDbHealthCheck : IHealthCheck
    {
        private readonly IDatabaseService _databaseService;

        public MongoDbHealthCheck(IDatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            // In this case we're not only checking that we can successfully read the database,
            // but we are also testing that the collection is present and contains seed data
            var data = await _databaseService.GetCountries();
            if (data == null || !data.Any())
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, "Could not contact database and/or read data");
            }

            return new HealthCheckResult(HealthStatus.Healthy);
        }
    }
}
