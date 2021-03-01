using System;
using InfoTrack.Orders.Api.Client;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Order info
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// Order Id
        /// </summary>
        int OrderId { get; set; }

        /// <summary>
        /// Parent order id (if any)
        /// </summary>
        int? ParentOrderId { get; set; }

        /// <summary>
        /// Order description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Date the order was placed
        /// </summary>
        DateTimeOffset DateOrdered { get; set; }

        /// <summary>
        /// Date the order was completed
        /// </summary>
        DateTimeOffset? DateCompleted { get; set; }

        /// <summary>
        /// Order status
        /// </summary>
        SystemStatusEnum Status { get; set; } // TODO this should be ApplicationStatus. Platform have raised a PR to fix.

        /// <summary>
        /// Status message
        /// </summary>
        string StatusMessage { get; set; }

        /// <summary>
        /// Client reference (aka "matter")
        /// </summary>
        string ClientReference { get; set; }

        /// <summary>
        /// Retailer reference
        /// </summary>
        string RetailerReference { get; set; }
    }
}
