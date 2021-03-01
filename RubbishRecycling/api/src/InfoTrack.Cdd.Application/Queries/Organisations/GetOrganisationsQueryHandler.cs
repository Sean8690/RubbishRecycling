using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Common.Application;

namespace InfoTrack.Cdd.Application.Queries.Organisations
{
    /// <summary>
    /// Get a list of organisations matching name/number/country
    /// </summary>
    public class GetOrganisationsQueryHandler : IQueryHandler<GetOrganisationsQuery, IEnumerable<OrganisationLiteDto>>
    {
        private readonly IOrganisationService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Get a list of organisations matching name/number/country
        /// </summary>
        public GetOrganisationsQueryHandler(IOrganisationService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #pragma warning disable 1591
        public async Task<IEnumerable<OrganisationLiteDto>> Handle(GetOrganisationsQuery request, CancellationToken cancellationToken)
        #pragma warning restore 1591
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            IEnumerable<OrganisationLite> organisations = await _service.GetOrganisationsAsync(request.Name, request.Number, request.KyckrCountryCode);
            return _mapper.Map<List<OrganisationLiteDto>>(organisations);
        }
    }
}
