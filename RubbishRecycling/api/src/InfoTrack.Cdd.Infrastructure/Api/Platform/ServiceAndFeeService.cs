using System;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Services.Api.Client;
using Microsoft.Extensions.Logging;

namespace InfoTrack.Cdd.Infrastructure.Api.Platform
{
    /// <summary>
    /// Interrogate Platform.Core ServicesApi
    /// </summary>
    public class ServiceAndFeeService : PlatformServiceBase, IServicesService, IQuoteService
    {
        private readonly IServicesApiClient _client;

        public ServiceAndFeeService(IServicesApiClient client, ILogger<OrderService> logger) : base(logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Get the numeric ServiceId associated with the given ServiceIdentifier
        /// </summary>
        public async Task<int> GetServiceIdentifier(ServiceIdentifier serviceIdentifier, CancellationToken cancellationToken)
        {
            return (await CallAuthority(serviceIdentifier.ToString(), _client.Services_GetByIdentifierAsync, cancellationToken)).ServiceId;
        }

        /// <summary>
        /// Get a fee quote for a given ServiceIdentifier and KyckrCountryCode
        /// </summary>
        public async Task<Quote> GetFeeAsync(ServiceIdentifier serviceIdentifier, string kyckrCountryCode, CancellationToken cancellationToken)
        {
            if (serviceIdentifier == ServiceIdentifier.Undefined)
            {
                throw new ArgumentNullException(nameof(serviceIdentifier));
            }
            var service = await GetServiceIdentifier(serviceIdentifier, cancellationToken);
            var command = new CreateQuoteCommand { ServiceId = service };

            var quoteResponse = await CallAuthority(command, _client.Quotes_CreateAsync, cancellationToken);
            return new Quote
            {
                Fee = quoteResponse.SaleTotal,
                QuoteId = quoteResponse.QuoteId,
                ServiceIdentifier = serviceIdentifier
            };
        }
    }
}
