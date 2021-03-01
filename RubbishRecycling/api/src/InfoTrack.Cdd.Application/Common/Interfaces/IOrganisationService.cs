using System.Collections.Generic;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for getting organisation data (lookup queries and detail searches)
    /// </summary>
    public interface IOrganisationService
    {
        /// <summary>
        /// Check whether the service is available
        /// </summary>
        Task<bool> PingAsync();

        /// <summary>
        /// Get a list of organisations matching name/number/country
        /// </summary>
        Task<IEnumerable<OrganisationLite>> GetOrganisationsAsync(string name, string number, string kyckrCountryCode);

        /// <summary>
        /// Get a single organisation matching providerEntityCode
        /// </summary>
        Task<Models.Organisation> GetOrganisationAsync(string providerEntityCode, string kyckrCountryCode, int orderId, ServiceIdentifier serviceIdentifier);
    }
}
