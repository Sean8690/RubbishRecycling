using System;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for getting person data (lookup queries and detail searches)
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Check whether the service is available
        /// </summary>
        Task<bool> PingAsync();

        /// <summary>
        /// Get a list of persons matching name/number/country
        /// </summary>
        Task<PersonMatches> GetPersonsAsync(string givenName, string middleName, string familyName, string dob, string yearOfBirth, int orderId);

        Task<Person> GetPersonReportAsync(int parentOrderId, ServiceIdentifier serviceIdentifier, string providerEntityCode, int childOrderId, CancellationToken cancellationToken);
    }
}
