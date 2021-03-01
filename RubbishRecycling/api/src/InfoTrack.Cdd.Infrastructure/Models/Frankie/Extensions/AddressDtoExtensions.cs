using System;
using System.Collections.Generic;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared;
using System.Linq;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class AddressDtoExtensions
    {
        public static bool HasData(this AddressDto address)
        {
            // This ignores Type and TypeCode because it's assumed if they are the only properties
            // then there the data has no value
            return address.HasAddress() || address.HasContact();
        }

        public static bool HasAddress(this AddressDto address)
        {
            return address != null && (!string.IsNullOrWhiteSpace(address.AddressInOneLine)
                   || !string.IsNullOrWhiteSpace(address.AddressLine1)
                   || !string.IsNullOrWhiteSpace(address.AddressLine2)
                   || !string.IsNullOrWhiteSpace(address.AddressLine3)
                   || !string.IsNullOrWhiteSpace(address.AddressLine4)
                   || !string.IsNullOrWhiteSpace(address.AddressLine5)
                   || !string.IsNullOrWhiteSpace(address.CityTown)
                   || !string.IsNullOrWhiteSpace(address.Country)
                   || !string.IsNullOrWhiteSpace(address.Postcode)
                   || !string.IsNullOrWhiteSpace(address.RegionState)
                   || (address.Lines != null && address.Lines.Any(line => !string.IsNullOrWhiteSpace(line?.Line))));
        }

        public static string ToTypeString(this AddressDto address)
        {
            var hasType = !string.IsNullOrWhiteSpace(address?.Type);
            var hasTypeCode = !string.IsNullOrWhiteSpace(address?.TypeCode);
            if (!hasType && !hasTypeCode)
            {
                return "Not specified";
            }
            else if (hasType && hasTypeCode)
            {
                return $"{address.Type} ({address.TypeCode})";
            }
            else if (hasType)
            {
                return address.Type;
            }
            return address.TypeCode;
        }

        public static string ToAddressString(this AddressDto address)
        {
            if (address == null || !address.HasAddress())
            {
                return null;
            }
            else if (!string.IsNullOrWhiteSpace(address.AddressInOneLine))
            {
                return address.AddressInOneLine;
            }

            List<string> addressLineElements = null;
            if (address.Lines != null && address.Lines.Any(l => !string.IsNullOrWhiteSpace(l.Line)))
            {
                addressLineElements = address.Lines
                    .Where(l => !string.IsNullOrWhiteSpace(l?.Line))
                    .Select(l => l.Line).ToList();
            }

            List<string> addressElements = new List<string>();
            if (!string.IsNullOrWhiteSpace(address.AddressLine1))
            {
                addressElements.Add(address.AddressLine1);
            }
            if (!string.IsNullOrWhiteSpace(address.AddressLine2))
            {
                addressElements.Add(address.AddressLine2);
            }
            if (!string.IsNullOrWhiteSpace(address.AddressLine3))
            {
                addressElements.Add(address.AddressLine3);
            }
            if (!string.IsNullOrWhiteSpace(address.AddressLine4))
            {
                addressElements.Add(address.AddressLine4);
            }
            if (!string.IsNullOrWhiteSpace(address.AddressLine5))
            {
                addressElements.Add(address.AddressLine5);
            }
            if (!string.IsNullOrWhiteSpace(address.CityTown))
            {
                addressElements.Add(address.CityTown);
            }
            if (!string.IsNullOrWhiteSpace(address.RegionState))
            {
                addressElements.Add(address.RegionState);
            }
            if (!string.IsNullOrWhiteSpace(address.Postcode))
            {
                addressElements.Add(address.Postcode);
            }
            if (!string.IsNullOrWhiteSpace(address.Country))
            {
                addressElements.Add(address.Country);
            }

            if (addressLineElements == null || !addressLineElements.Any())
            {
                return string.Join(" ", addressElements);
            } 
            else if (!addressElements.Any())
            {
                return string.Join(" ", addressLineElements);
            }
            else
            {
                var addressLineHasNoUniqueElements = addressLineElements.All(line => 
                    addressElements.Any(prop => prop != null && prop.Equals(line, StringComparison.OrdinalIgnoreCase)));
                
                if (addressLineHasNoUniqueElements) // often but not always duplicated (depends on the country)
                {
                    return string.Join(" ", addressElements);
                }
                else
                {
                    return string.Join(" ", addressLineElements.Union(addressElements));
                }
            }
        }

        public static bool HasContact(this AddressDto address)
        {
            return address != null && (!string.IsNullOrWhiteSpace(address.Email)
                   || !string.IsNullOrWhiteSpace(address.FaxNumber)
                   || !string.IsNullOrWhiteSpace(address.TelephoneNumber)
                   || !string.IsNullOrWhiteSpace(address.WebsiteUrl));
        }
    }
}
