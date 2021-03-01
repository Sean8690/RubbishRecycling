using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Parties.Api.Client;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for working with parties
    /// </summary>
    public interface IPartiesService
    {
        /// <summary>
        /// Get the RetailerType for the given RetailerId
        /// </summary>
        Task<RetailerTypeEnum?> GetRetailerTypeAsync(int retailerId, CancellationToken cancellationToken);
    }
}
