using System;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Api.IntegrationTests.Mocks
{
    public class TestQuoteService : IQuoteService
    {
        public async Task<Quote> GetFeeAsync(ServiceIdentifier serviceIdentifier, string kyckrCountryCode, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new Quote
            {
                Fee = new decimal(22.50),
                ServiceIdentifier = serviceIdentifier,
                KyckrCountryCode = kyckrCountryCode,
                QuoteId = Guid.NewGuid()
            });
        }
    }
}
