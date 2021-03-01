using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Commands.Order;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Orders.Api.Client;
using InfoTrack.Platform.Core.Orders.Contracts;
using InfoTrack.Storage.Contracts.Api.v2;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace InfoTrack.Cdd.Api.IntegrationTests.Mocks
{
    public class TestOrderService : IOrderService
    {
        public async Task<OrderDto> OrderAsync<TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : Request, IOrderRequest
        {
            return await Task.FromResult(new OrderDto
            {
                OrderId = 123456,
                Description = "Test order description",
                Files = new ObservableCollection<FileDto>()
            });
        }

        public async Task SetResponseAsync<TAuthorityResponse>(int orderId, TAuthorityResponse authorityResponse, CancellationToken cancellationToken) where TAuthorityResponse : class
        {
        }

        public async Task<List<object>> GetResponseAsync(int orderId, CancellationToken cancellationToken)
        {
            return new List<object>();
        }

        public async Task AttachFileAsync(int orderId, FileMetadata file, CancellationToken cancellationToken)
        {
        }

        public async Task TryAttachAuthorityResponseFileAsync(int orderId, FileMetadata file, CancellationToken cancellationToken)
        {
        }

        public async Task UpdateDescriptionAsync(int orderId, string description, CancellationToken cancellationToken)
        {
        }

        public async Task CompleteAsync(int orderId, string statusMessage, CancellationToken cancellationToken)
        {
        }

        public async Task ErrorAsync(int orderId, string errorMessage, CancellationToken cancellationToken)
        {
        }

        public async Task<IEnumerable<Order>> GetMatchingOrdersAsync<TRequest>(ServiceIdentifier serviceIdentifier, string clientReference, TRequest orderRequest, CancellationToken cancellationToken)
            where TRequest : Request, IOrderRequest, IMatches<TRequest>
        {
            return await Task.FromResult(new List<Order>{ new Order
            {
                OrderId = 123456,
                Description = "Test order description",
                ClientReference = "cdd-integration",
                RetailerReference = "SMK_32ec0e22-4026-45aa-8e68-25df308b9ace_c77eae60-b797-44c2-ae49-cf9adb7c4bec_Matter_45f5b097-913a-4dd5-9ec0-834a4f4ac67f_26fe2d4a-9e6e-4aac-98fe-1785f3040e6c"
            }});
        }

        public async Task<Order> GetAsync<TRequest>(int orderId, CancellationToken cancellationToken) where TRequest : Request, IOrderRequest
        {
            return await Task.FromResult(new Order
            {
                OrderId = 123456,
                Description = "Test order description",
                ClientReference = "cdd-integration",
                RetailerReference = "SMK_32ec0e22-4026-45aa-8e68-25df308b9ace_c77eae60-b797-44c2-ae49-cf9adb7c4bec_Matter_45f5b097-913a-4dd5-9ec0-834a4f4ac67f_26fe2d4a-9e6e-4aac-98fe-1785f3040e6c"
            });
        }

        public Task<IEnumerable<OrderResponse>> OrderAsync<TRequest>(IEnumerable<TRequest> requests, CancellationToken cancellationToken) where TRequest : OrderRequest, IOrderRequest
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> GetResponseAsync<TResponse>(int orderId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TRequest> GetRequestAsync<TRequest>(int orderId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> OrderAsync<TRequest>(TRequest request, CancellationToken cancellationToken, ApplicationStatusEnum? desiredStatus = null) where TRequest : Request, IOrderRequest
        {
            throw new NotImplementedException();
        }
    }
}
