using System;
using AutoMapper;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Common.Application.Automapper;
using InfoTrack.Orders.Api.Client;

namespace InfoTrack.Cdd.Application.Dtos
{
    /// <summary>
    /// Order info
    /// </summary>
    public class OrderDto : IMapFrom<IOrder>, IOrder
    {
        /// <summary>
        /// Order Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Parent order id (if any)
        /// </summary>
        public int? ParentOrderId { get; set; }

        /// <summary>
        /// Order description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date the order was placed
        /// </summary>
        public DateTimeOffset DateOrdered { get; set; }

        /// <summary>
        /// Date the order was completed
        /// </summary>
        public DateTimeOffset? DateCompleted { get; set; }

        /// <summary>
        /// Order status
        /// </summary>
        public SystemStatusEnum Status { get; set; } // TODO this should be ApplicationStatus. Platform have raised a PR to fix.

        /// <summary>
        /// Status message
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Client reference (aka "matter")
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        /// Retailer reference
        /// </summary>
        public string RetailerReference { get; set; }

        #pragma warning disable 1591
        public void Mapping(Profile profile)
        #pragma warning restore 1591
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            profile.CreateMap<Models.Order, OrderDto>();
        }
    }
}
