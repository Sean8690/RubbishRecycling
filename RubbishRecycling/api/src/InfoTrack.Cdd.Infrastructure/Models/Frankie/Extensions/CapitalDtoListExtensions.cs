using System.Collections.Generic;
using System.Linq;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class CapitalDtoListExtensions
    {
        public static bool HasAnyAmount(this List<CapitalDto> capitals) 
            => capitals != null && capitals.Any(a => !string.IsNullOrWhiteSpace(a?.Amount));

        public static bool HasAnyCurrency(this List<CapitalDto> capitals)
            => capitals != null && capitals.Any(a => !string.IsNullOrWhiteSpace(a?.Currency));

        public static bool HasAnyType(this List<CapitalDto> capitals)
            => capitals != null && capitals.Any(a => !string.IsNullOrWhiteSpace(a?.Type));

        public static bool HasAnyTypeCode(this List<CapitalDto> capitals)
            => capitals != null && capitals.Any(a => !string.IsNullOrWhiteSpace(a?.TypeCode));
    }
}
