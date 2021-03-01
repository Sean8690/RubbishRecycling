using System;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Order request
    /// </summary>
    public interface IOrderRequest
    {
        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        Common.Enums.ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// QuoteId (optional)
        /// </summary>
        Guid? QuoteId { get; set; }
    }
}
