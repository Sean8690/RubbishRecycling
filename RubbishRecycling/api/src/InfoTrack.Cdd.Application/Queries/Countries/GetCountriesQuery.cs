using System.Collections.Generic;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Common.Application;

namespace InfoTrack.Cdd.Application.Queries.Countries
{
    /// <summary>
    /// Get a list of supported countries
    /// </summary>
    public class GetCountriesQuery : IQuery<IEnumerable<CountryDto>>
    {
        /// <summary>
        /// Get a list of supported countries
        /// </summary>
        public GetCountriesQuery()
        {
        }

    }
}
