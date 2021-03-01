using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Common.Application;

namespace InfoTrack.Cdd.Application.Queries.Countries
{
    /// <summary>
    /// Get a list of supported countries
    /// </summary>
    public class GetCountriesQueryHandler : IQueryHandler<GetCountriesQuery, IEnumerable<CountryDto>>
    {
        private readonly ICountryService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Get a list of supported countries
        /// </summary>
        public GetCountriesQueryHandler(ICountryService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #pragma warning disable 1591
        public async Task<IEnumerable<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        #pragma warning restore 1591
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var countries = await _service.GetCountriesAsync();
            return _mapper.Map<List<CountryDto>>(countries);
        }
    }
}
