using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Orders.Api.Client;
using InfoTrack.Platform.Core.Orders.Contracts;
using InfoTrack.Storage.Contracts.Api.v2;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for placing orders
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Place an order
        /// </summary>
        Task<OrderDto> OrderAsync<TRequest>(TRequest request, CancellationToken cancellationToken, ApplicationStatusEnum? desiredStatus = null)
            where TRequest : Request, IOrderRequest;

        /// <summary>
        /// Place a batch of orders
        /// </summary>
        Task<IEnumerable<OrderResponse>> OrderAsync<TRequest>(IEnumerable<TRequest> requests, CancellationToken cancellationToken)
            where TRequest : OrderRequest, IOrderRequest;

        /// <summary>
        /// Add an authority response object to an order
        /// </summary>
        Task SetResponseAsync<TAuthorityResponse>(int orderId, TAuthorityResponse authorityResponse, CancellationToken cancellationToken)
            where TAuthorityResponse : class;

        /// <summary>
        /// Read the request from an order
        /// </summary>
        Task<TRequest> GetRequestAsync<TRequest>(int orderId, CancellationToken cancellationToken);

        /// <summary>
        /// Read an authority response object to an order
        /// </summary>
        Task<TResponse> GetResponseAsync<TResponse>(int orderId, CancellationToken cancellationToken);

        /// <summary>
        /// Attach a file to an order
        /// </summary>
        Task AttachFileAsync(int orderId, FileMetadata file, CancellationToken cancellationToken);

        /// <summary>
        /// Attach a (hidden) AuthorityResponse file to an order. On failure, logs an error and does not throw any exceptions.
        /// </summary>
        Task TryAttachAuthorityResponseFileAsync(int orderId, FileMetadata file, CancellationToken cancellationToken);

        /// <summary>
        /// Update the description on an order
        /// </summary>
        Task UpdateDescriptionAsync(int orderId, string description, CancellationToken cancellationToken);

        /// <summary>
        /// Mark an order as completed
        /// </summary>
        Task CompleteAsync(int orderId, string statusMessage, CancellationToken cancellationToken);

        /// <summary>
        /// Mark an order as errored
        /// </summary>
        Task ErrorAsync(int orderId, string errorMessage, CancellationToken cancellationToken);

        /// <summary>
        /// Get a list of matching orders (e.g. for reorder detection)
        /// </summary>
        Task<IEnumerable<Order>> GetMatchingOrdersAsync<TRequest>(ServiceIdentifier serviceIdentifier, string clientReference, TRequest orderRequest, CancellationToken cancellationToken)
            where TRequest : Request, IOrderRequest, IMatches<TRequest>;

        /// <summary>
        /// Get an order by Order Id
        /// </summary>
        Task<Order> GetAsync<TRequest>(int orderId, CancellationToken cancellationToken)
            where TRequest : Request, IOrderRequest;
    }
}
