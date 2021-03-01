using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Common.Application;

namespace InfoTrack.Cdd.Application.Commands.Quote
{
    /// <summary>
    /// Get a fee quote
    /// </summary>
    public class QuoteCommandHandler : ICommandHandler<QuoteCommand, QuoteDto>
    {
        private readonly IQuoteService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Get a fee quote
        /// </summary>
        public QuoteCommandHandler(IQuoteService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #pragma warning disable 1591
        public async Task<QuoteDto> Handle(QuoteCommand request, CancellationToken cancellationToken)
        #pragma warning restore 1591
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var quote = await _service.GetFeeAsync(request.ServiceIdentifier, request.KyckrCountryCode, cancellationToken);
            return _mapper.Map<QuoteDto>(quote);
        }
    }
}
