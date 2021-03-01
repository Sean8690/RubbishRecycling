using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// This interface enables authority data to be mocked without mocking the authority service,
    /// therefore enabling all logic to be tested with canned data used for cost purposes
    /// </summary>
    public interface ITestableOrganisationClient
    {
        /// <summary>
        /// Check whether the service is available
        /// </summary>
        /// <returns></returns>
        Task<TestableClientResponse> PingAsync();

        /// <summary>
        /// Get a list of organisations
        /// </summary>
        Task<TestableClientResponse> GetOrganisationsAsync<TRequest>(TRequest request);

        /// <summary>
        /// Get a single organisation
        /// </summary>
        Task<TestableClientResponse> GetOrganisationAsync<TRequest>(TRequest request);
    }
}
