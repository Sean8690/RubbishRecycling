using System.Collections.Generic;
using InfoTrack.Cdd.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace InfoTrack.Cdd.Api.Examples
{
    public class CountryDtoListExample : IExamplesProvider<IEnumerable<CountryDto>>
    {
        public virtual IEnumerable<CountryDto> GetExamples()
        {
            return new List<CountryDto>
            {
                new CountryDto {CountryName = "Australia", Iso2 = "AU", KyckrCountryCode = "AU"}, 
                new CountryDto {CountryName = "New Zealand", Iso2 = "NZ", KyckrCountryCode = "NZ"}, 
                new CountryDto {CountryName = "United Kingdom", Iso2 = "GB", KyckrCountryCode = "GB"}
            } as IEnumerable<CountryDto>;
        }
    }
}
