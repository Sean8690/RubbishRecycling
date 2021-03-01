using System;

namespace InfoTrack.Cdd.Application.Models
{
    /// <summary>
    /// Fee quote info
    /// </summary>
    public class Quote 
    {
        /// <summary>
        /// Fee
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        public Common.Enums.ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }

        /// <summary>
        /// QuoteId
        /// </summary>
        public Guid QuoteId { get; set; }
    }
}
