using System.Collections.Generic;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Common.Application;

namespace InfoTrack.Cdd.Application.Queries.Orders
{
    /// <summary>
    /// Get a list of orders matching serviceIdentifier/providerEntityCode/kyckrCountryCode/clientReference
    /// </summary>
    public class GetOrdersQuery : IQuery<IEnumerable<OrderDto>>
    {
        /// <summary>
        /// Get a list of orders matching serviceIdentifier/providerEntityCode/kyckrCountryCode/clientReference
        /// </summary>
        public GetOrdersQuery(ServiceIdentifier serviceIdentifier, string providerEntityCode, string kyckrCountryCode,
            string clientReference, string retailerReference)
        {
            ServiceIdentifier = serviceIdentifier;
            ProviderEntityCode = providerEntityCode;
            KyckrCountryCode = kyckrCountryCode;
            ClientReference = clientReference;
            RetailerReference = retailerReference;
        }

        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        public ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// Provider's unique organisation identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }

        /// <summary>
        /// Client reference (aka "matter")
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        /// Retailer reference for the client
        /// </summary>
        public string RetailerReference { get; set; }
    }
}
