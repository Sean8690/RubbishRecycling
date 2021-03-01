using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos.Order;
using InfoTrack.Common.Application;

namespace InfoTrack.Cdd.Application.Commands.PersonOrder
{
    /// <summary>
    /// 
    /// </summary>
    public class PersonOrderRequestCommandHandler : ICommandHandler<PersonOrderRequestCommand, PersonListResponseDto>
    {
        private readonly IServiceFacade _service;

        /// <summary>
        /// 
        /// </summary>
        public PersonOrderRequestCommandHandler(IServiceFacade service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        #pragma warning disable 1591
        public async Task<PersonListResponseDto> Handle(PersonOrderRequestCommand request, CancellationToken cancellationToken)
        #pragma warning restore 1591
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _service.OrderPersonSummaryAsync(request, cancellationToken);
        }
    }
}
