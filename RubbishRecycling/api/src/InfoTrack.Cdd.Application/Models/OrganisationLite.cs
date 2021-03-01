using InfoTrack.Cdd.Application.Common.Interfaces;

namespace InfoTrack.Cdd.Application.Models
{
    /// <summary>
    /// Basic organisation info.
    ///
    /// More info is available from the provider if required (e.g. aliases, addresses), but the structure of this object has been kept simple for now
    /// </summary>
    public class OrganisationLite : IOrganisation
    {
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

        /// <summary>
        /// Provider's unique organisation identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// Organisation registered address.
        ///
        /// There may actually be a list of addresses, and structured data is available if necessary. So the API contract for this property can be changed if required.
        /// </summary>
        public string Address { get; set; }
    }
}
