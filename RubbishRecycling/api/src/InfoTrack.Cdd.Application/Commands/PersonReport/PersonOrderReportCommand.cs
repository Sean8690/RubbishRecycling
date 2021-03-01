using System;
using System.Collections.Generic;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Common.Application;
using InfoTrack.Platform.Core.Orders.Contracts;

namespace InfoTrack.Cdd.Application.Commands.PersonReport
{
    /// <summary>
    /// Person report (e.g. risk & compliance)
    /// </summary>
    public class PersonOrderReportCommand : Request, ICommand<List<OrderDto>>, IOrderRequest
    {
        /// <summary>
        /// Person report (e.g. Risk & Compliance)
        /// </summary>
        public PersonOrderReportCommand(int orderId, IEnumerable<string> providerEntityCodes, ServiceIdentifier serviceIdentifier, Guid? quoteId)
        {
            OrderId = orderId;
            ProviderEntityCodes = providerEntityCodes;
            ServiceIdentifier = serviceIdentifier;
            QuoteId = quoteId;
        }

        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        public ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// QuoteId (optional)
        /// </summary>
        public Guid? QuoteId { get; set; }

        /// <summary>
        /// Provider's unique entity identifiers (current provider is Frankie Financial)
        /// </summary>
        public IEnumerable<string> ProviderEntityCodes { get; set; }
    }
}
