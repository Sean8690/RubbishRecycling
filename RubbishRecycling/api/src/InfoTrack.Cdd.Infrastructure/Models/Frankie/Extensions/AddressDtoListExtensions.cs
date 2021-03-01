using System.Collections.Generic;
using System.Linq;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class AddressDtoListExtensions
    {
        public static bool HasAnyPhone(this List<AddressDto> addresses) 
            => addresses != null && addresses.Any(a => !string.IsNullOrWhiteSpace(a?.TelephoneNumber));

        public static bool HasAnyFax(this List<AddressDto> addresses)
            => addresses != null && addresses.Any(a => !string.IsNullOrWhiteSpace(a?.FaxNumber));

        public static bool HasAnyEmail(this List<AddressDto> addresses)
            => addresses != null && addresses.Any(a => !string.IsNullOrWhiteSpace(a?.Email));

        public static bool HasAnyWebsite(this List<AddressDto> addresses)
            => addresses != null && addresses.Any(a => !string.IsNullOrWhiteSpace(a?.WebsiteUrl));

        public static bool HasAnyType(this List<AddressDto> addresses)
            => addresses != null && addresses.Any(a => !string.IsNullOrWhiteSpace(a?.Type) || !string.IsNullOrWhiteSpace(a.TypeCode));

        public static string ToAddressString(this List<AddressDto> addresses)
            => addresses == null ? null : string.Join("; ", addresses.Select(a => a?.ToAddressString()));

    }
}
