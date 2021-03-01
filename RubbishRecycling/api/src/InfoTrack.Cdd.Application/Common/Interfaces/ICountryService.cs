using System.Collections.Generic;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for getting country data 
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Get a list of supported countries
        /// </summary>
        Task<IEnumerable<Country>> GetCountriesAsync();

        /// <summary>
        /// Get a country by KyckrCountryCode
        /// </summary>
        Task<Country> GetCountryAsync(string kyckrCountryCode);
    }
}
