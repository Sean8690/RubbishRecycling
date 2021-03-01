using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for managing fee quotes
    /// </summary>
    public interface IQuoteService
    {
        /// <summary>
        /// Place an order
        /// </summary>
        Task<Quote> GetFeeAsync(ServiceIdentifier serviceIdentifier, string kyckrCountryCode, CancellationToken cancellationToken);
    }
}
