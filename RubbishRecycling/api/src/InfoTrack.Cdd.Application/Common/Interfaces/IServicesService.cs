using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for interrogating services
    /// </summary>
    public interface IServicesService
    {
        /// <summary>
        /// Get the numeric ServiceId associated with the given ServiceIdentifier
        /// </summary>
        Task<int> GetServiceIdentifier(ServiceIdentifier serviceIdentifier, CancellationToken cancellationToken);
    }
}
