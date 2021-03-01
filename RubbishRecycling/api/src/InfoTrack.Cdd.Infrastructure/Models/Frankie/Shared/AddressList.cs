using System.Collections.Generic;
using InfoTrack.Cdd.Infrastructure.Utils;
using Newtonsoft.Json;

#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
// ReSharper disable UnusedMember.Global

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared
{
    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class AddressDto
    {
        public string AddressInOneLine { get; set; }

        /// <summary>
        /// Duplicated (AddressLine1 == Lines.AddressLineDTO[0].Line)
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Duplicated (AddressLine2 == Lines.AddressLineDTO[1].Line)
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Duplicated (AddressLine3 == Lines.AddressLineDTO[2].Line)
        /// </summary>
        public string AddressLine3 { get; set; }

        /// <summary>
        /// Duplicated (AddressLine4 == Lines.AddressLineDTO[3].Line)
        /// </summary>
        public string AddressLine4 { get; set; }

        /// <summary>
        /// Duplicated (AddressLine5 == Lines.AddressLineDTO[4].Line)
        /// </summary>
        public string AddressLine5 { get; set; }

        /// <summary>
        /// May also be present in address lines (e.g. AddressLine3 or Line.AddressLineDTO[2].Line)
        /// </summary>
        public string CityTown { get; set; }

        /// <summary>
        /// May also be present in address lines (similar to CityTown)
        /// </summary>
        public string Country { get; set; }

        public string Email { get; set; }
        
        public string FaxNumber { get; set; }

        [JsonProperty("Line"), JsonWrappedObjectList("AddressLineDTO")]
        public List<AddressLineDto> Lines { get; set; }

        /// <summary>
        /// May also be present in address lines (similar to CityTown)
        /// </summary>
        public string Postcode { get; set; }
        
        /// <summary>
        /// e.g. "VIC"
        /// </summary>
        public string RegionState { get; set; }

        public string TelephoneNumber { get; set; }
        
        /// <summary>
        /// e.g. "Registered Office (from 10/06/2009)"
        /// </summary>
        public string Type { get; set; }

        public string TypeCode { get; set; }

#pragma warning disable CA1056 // Uri properties should not be strings
        public string WebsiteUrl { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings
    }

    public class AddressLineDto
    {
        public string Line { get; set; }
        public string Type { get; set; }
        public string TypeCode { get; set; }
    }
}
