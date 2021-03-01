using System.Collections.Generic;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Common.Application;

namespace InfoTrack.Cdd.Application.Queries.Organisations
{
    /// <summary>
    /// Get a list of organisations matching name/number/country
    /// </summary>
    public class GetOrganisationsQuery : IQuery<IEnumerable<OrganisationLiteDto>>, IOrganisation
    {
        /// <summary>
        /// Get a list of organisations matching name/number/country
        /// </summary>
        public GetOrganisationsQuery(string name, string number, string country)
        {
            Name = name;
            Number = number;
            KyckrCountryCode = country;
        }

        /// <summary>
        /// Organisation name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Organisation number, e.g. ABN or ACN
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }
    }
}
