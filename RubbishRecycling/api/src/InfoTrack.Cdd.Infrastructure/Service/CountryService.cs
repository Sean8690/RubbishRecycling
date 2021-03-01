using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Infrastructure.Service
{
    /// <summary>
    /// Get country data 
    /// </summary>
    public class CountryService : ICountryService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMapper _mapper;

        public CountryService(IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            var countries = await _databaseService.GetCountries();
            return _mapper.Map<IEnumerable<Country>>(countries);
        }

        public async Task<Country> GetCountryAsync(string kyckrCountryCode)
        {
            var country = await _databaseService.GetCountry(kyckrCountryCode);
            return _mapper.Map<Country>(country);
        }
    }
}
