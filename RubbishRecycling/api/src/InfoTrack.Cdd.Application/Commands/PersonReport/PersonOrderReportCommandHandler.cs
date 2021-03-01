using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Common.Application;
using MediatR;

namespace InfoTrack.Cdd.Application.Commands.PersonReport
{
    /// <summary>
    /// 
    /// </summary>
    public class PersonOrderReportCommandHandler : ICommandHandler<PersonOrderReportCommand, List<OrderDto>>
    {
        private readonly IServiceFacade _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        public PersonOrderReportCommandHandler(IServiceFacade service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

#pragma warning disable 1591
        public async Task<List<OrderDto>> Handle(PersonOrderReportCommand request, CancellationToken cancellationToken)
#pragma warning restore 1591
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _service.OrderPersonReportsAsync(request, cancellationToken);
        }
    }
}
