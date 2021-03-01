using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfoTrack.Cdd.Application.Commands.Order;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Common.Application;
using OrderDto = InfoTrack.Cdd.Application.Dtos.OrderDto;

namespace InfoTrack.Cdd.Application.Queries.Orders
{
    /// <summary>
    /// Get a list of organisations matching name/number/country
    /// </summary>
    public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Get a list of organisations matching name/number/country
        /// </summary>
        public GetOrdersQueryHandler(IOrderService service,  IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #pragma warning disable 1591
        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        #pragma warning restore 1591
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var orderRequest = _mapper.Map<OrganisationOrderRequestCommand>(request);

            var orders = await _service.GetMatchingOrdersAsync<OrganisationOrderRequestCommand>(request.ServiceIdentifier, request.ClientReference, orderRequest, cancellationToken);
            
            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
