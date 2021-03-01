using System;

namespace InfoTrack.Cdd.Application.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class AmlPersonLookupResponse
    {
        /// <summary>
        /// Provider's unique entity identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Dob { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Order by confidence score and then alphabetically and then DOB.
        /// </summary>
        public double ConfidenceScore { get; set; }
    }
}
