using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfoTrack.Cdd.Application.Commands.PersonOrder;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Cdd.Infrastructure.Models;
using InfoTrack.Orders.Api.Client;
using InfoTrack.Platform.Core.Orders.Contracts;
using InfoTrack.Storage.Contracts.Api.v2;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace InfoTrack.Cdd.Infrastructure.Api.Platform
{
    /// <summary>
    /// Interrogate Platform.Core OrdersApi
    /// </summary>
    public class OrderService : PlatformServiceBase, IOrderService
    {
        private readonly IOrdersApiClient _client;
        private readonly IServicesService _servicesService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrdersApiClient client, IServicesService servicesService, IMapper mapper, ILogger<OrderService> logger) : base(logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _servicesService = servicesService ?? throw new ArgumentNullException(nameof(servicesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OrderDto> OrderAsync<TRequest>(TRequest request, CancellationToken cancellationToken, ApplicationStatusEnum? desiredStatus = null)
            where TRequest : Request, IOrderRequest
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var serviceId = await _servicesService.GetServiceIdentifier(request.ServiceIdentifier, cancellationToken);
            var command = new CreateOrderCommand
            {
                Request = request,
                ClientReference = request.ClientReference,
                ServiceId = serviceId,
                RetailerReference = request.RetailerReference,
                Description = request.Description,
                QuoteId = request.QuoteId,
                DesiredApplicationStatus = desiredStatus
            };

            if (request.OrderId != null && request.OrderId.Value > 0)
            {
                var parent = await GetAsync<PersonOrderRequestCommand>(request.OrderId.Value, cancellationToken);
                command.ClientReference = parent.ClientReference;
                command.RetailerReference = parent.RetailerReference;

                // If there is a parent order, place this order as a child
                var results = await CallAuthority(request.OrderId.Value, new List<CreateOrderCommand> { command }, _client.Orders_CreateChildrenAsync, cancellationToken);
                return results?.Orders?.SingleOrDefault();
            }
            else
            {
                // If there is no parent order, just place this order as-is
                return await CallAuthority(command, _client.Orders_CreateAsync, cancellationToken);
            }
        }


        public async Task<IEnumerable<OrderResponse>> OrderAsync<TRequest>(IEnumerable<TRequest> requests, CancellationToken cancellationToken)
            where TRequest : OrderRequest, IOrderRequest
        {
            if (requests == null || !requests.Any())
            {
                throw new ArgumentNullException(nameof(requests));
            }

            // TODO batch this properly (can order multiple from platform in the one request, can get much more efficiency)
            var results = new List<OrderResponse>();
            foreach(var request in requests)
            {
                var order = await OrderAsync(request, cancellationToken);
                var mapped =  _mapper.Map<OrderResponse>(order);
                mapped.ProviderEntityCode = request.ProviderEntityCode;
                results.Add(mapped);
            }
            return results;
        }

        public async Task SetResponseAsync<TAuthorityResponse>(int orderId, TAuthorityResponse authorityResponse, CancellationToken cancellationToken)
            where TAuthorityResponse : class
        {
            if (orderId <= 0)
            {
                throw new ArgumentNullException(nameof(orderId));
            }
            if (authorityResponse == null)
            {
                throw new ArgumentNullException(nameof(authorityResponse));
            }

            // Platform will re-serialise the authority response. It will LOOK like the same response they sent us, but it 
            // won't be serialised using the custom serialiser (InternationalBusinessProfileResponse) so it won't be quite
            // the same. TODO this is a problem when it comes to testing, is there anything we can do about it?
            await CallAuthority(orderId, authorityResponse as object, _client.Orders_SetResponseAsync, cancellationToken);
        }

        public async Task<TRequest> GetRequestAsync<TRequest>(int orderId, CancellationToken cancellationToken)
        {
            if (orderId <= 0)
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            dynamic requestObject = await CallAuthority(orderId, _client.Orders_GetRequestObjectAsync, cancellationToken);
            var typedRequestObject = ((JObject)requestObject).ToObject<TRequest>();
            return typedRequestObject;
        }

        public async Task<TResponse> GetResponseAsync<TResponse>(int orderId, CancellationToken cancellationToken)
        {
            if (orderId <= 0)
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            var allResponses = await CallAuthority(orderId, _client.Orders_GetResponsesAsync, cancellationToken);

            // While multiple responses are available from platform, we know that for CDD we only set one so we only need to fetch one (and we know what type to exoect)
            var targetResponse = allResponses.SingleOrDefault();
            
            var typedObject = ((JObject)targetResponse).ToObject<TResponse>();
            return typedObject;
        }

        public async Task AttachFileAsync(int orderId, FileMetadata file, CancellationToken cancellationToken)
        {
            if (orderId <= 0)
            {
                throw new ArgumentNullException(nameof(orderId));
            }
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var command = new CreateFileCommand
            {
                Url = file.RetrievalUrl,
                Hidden = false,
                FriendlyName = file.Name,
                FileType = FileTypeEnum.AuthorityResponse
            };

            var contentLength = Convert.ToInt32(file.ContentLength);
            if (contentLength > 0)
            {
                command.ContentLength = contentLength;
            }

            await CallAuthority(orderId, command, _client.Orders_CreateFileAsync, cancellationToken);
        }

        public async Task TryAttachAuthorityResponseFileAsync(int orderId, FileMetadata file, CancellationToken cancellationToken)
        {
            if (orderId <= 0)
            {
                throw new ArgumentNullException(nameof(orderId));
            }
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var command = new CreateFileCommand
            {
                Url = file.RetrievalUrl,
                Hidden = true,
                FriendlyName = file.Name,
                FileType = FileTypeEnum.AuthorityResponse // is FileTypeEnum.Attachment more correct?
            };

            try
            {
                await CallAuthority(orderId, command, _client.Orders_CreateFileAsync, cancellationToken);
            }
            catch (Exception ex)
            {
                // Error should not interrupt control flow (e.g. should not prevent completion of an order)
                _logger.LogError(ex, "Unable to save authority response {AuthorityResponseUri}", file.RetrievalUrl);
            }
        }

        public async Task UpdateDescriptionAsync(int orderId, string description, CancellationToken cancellationToken)
        {
            if (orderId <= 0)
            {
                throw new ArgumentNullException(nameof(orderId));
            }
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            var command = new OrderTrackingRequestCommand
            {
                Description = description
            };

            await CallAuthority(orderId, command, _client.Orders_TrackingAsync, cancellationToken);
        }

        public async Task CompleteAsync(int orderId, string statusMessage, CancellationToken cancellationToken)
        {
            if (orderId <= 0)
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            var command = new OrderCompleteCommand
            {
                StatusMessage = statusMessage ?? "The order completed successfully."
            };

            await CallAuthority(orderId, command, _client.Orders_CompleteAsync, cancellationToken);
        }

        public async Task ErrorAsync(int orderId, string errorMessage, CancellationToken cancellationToken)
        {
            if (orderId <= 0)
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            var command = new CreateOrderErrorCommand
            {
                ErrorMessage = errorMessage ?? "There was an error processing your request."
            };

            await CallAuthority(orderId, command, _client.Orders_ErrorAsync, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetMatchingOrdersAsync<TRequest>(ServiceIdentifier serviceIdentifier, string clientReference, TRequest orderRequest, CancellationToken cancellationToken)
            where TRequest : Request, IOrderRequest, IMatches<TRequest>
        {
            if (serviceIdentifier == ServiceIdentifier.Undefined)
            {
                throw new ArgumentNullException(nameof(serviceIdentifier));
            }
            if (string.IsNullOrEmpty(clientReference))
            {
                throw new ArgumentNullException(nameof(clientReference));
            }
            if (orderRequest == null)
            {
                throw new ArgumentNullException(nameof(orderRequest));
            }

            var service = await _servicesService.GetServiceIdentifier(serviceIdentifier, cancellationToken);
            var command = new GetMatchingOrdersQuery
            {
                ClientReference = clientReference,
                ServiceId = service,
                ServiceRequest = orderRequest
            };

            var matchingOrdersResponse = await CallAuthority(command, _client.Orders_GetMatchingOrdersAsync, cancellationToken);

            var duplicateOrderTasks = matchingOrdersResponse.Select(async order =>
            {
                try
                {
                    // Idk why the original Orders_GetMatchingOrdersAsync call doesn't already filter for this, but it just doesn't
                    dynamic requestObject = await CallAuthority(order.OrderId, _client.Orders_GetRequestObjectAsync, cancellationToken);
                    var typedRequestObject = ((JObject)requestObject).ToObject<TRequest>();
                    if (!typedRequestObject.Matches(orderRequest))
                    {
                        return null;
                    }

                    // TODO this should be a implemented in IMapFrom with custom prop mapping
                    return new Order
                    {
                        OrderId = order.OrderId,
                        ParentOrderId = order.ParentOrderId == 0 ? null : order.ParentOrderId,
                        Status = order.SystemStatus,
                        StatusMessage = order.StatusMessage,
                        DateOrdered = order.Created,
                        DateCompleted = order.Completed,
                        ClientReference = clientReference,
                        RetailerReference = string.IsNullOrWhiteSpace(order.RetailerReference) ? null : order.RetailerReference,
                        Description = order.Description
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error when retrieving duplicate orders {OrderId} for {ClientReference} - {@OrderRequest}",
                        order.OrderId, clientReference, orderRequest);
                    return null;
                }
            }).ToList();

            await Task.WhenAll(duplicateOrderTasks);

            return duplicateOrderTasks
                .Select(task => task.Result)
                .Where(o => o != null)
                .OrderByDescending(o => o.DateOrdered);
        }

        public async Task<Order> GetAsync<TRequest>(int orderId, CancellationToken cancellationToken)
            where TRequest : Request, IOrderRequest
        {
            if (orderId <= 0)
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            OrderDto order = await CallAuthority(orderId, _client.Orders_GetAsync, cancellationToken);

            dynamic requestObject = await CallAuthority(order.OrderId, _client.Orders_GetRequestObjectAsync, cancellationToken);
            var typedRequestObject = ((JObject)requestObject).ToObject<TRequest>();

            // TODO this should be a implemented in IMapFrom with custom prop mapping
            return new Order
            {
                OrderId = order.OrderId,
                ParentOrderId = order.ParentOrderId == 0 ? null : order.ParentOrderId,
                Status = order.SystemStatus,
                StatusMessage = order.StatusMessage,
                DateOrdered = order.Created,
                DateCompleted = order.Completed,
                ClientReference = typedRequestObject.ClientReference,
                RetailerReference = string.IsNullOrWhiteSpace(order.RetailerReference) ? null : order.RetailerReference,
                Description = order.Description
            };
        }
    }
}
