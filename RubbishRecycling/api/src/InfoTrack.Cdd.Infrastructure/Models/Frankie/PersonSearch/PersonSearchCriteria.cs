using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch
{
    /// <summary>
    /// Frankie: 
    /// </summary>
    public static class PersonSearchCriteria
    {
        public class DateOfBirth
        {
            /// <summary>
            /// Frankie: 
            /// </summary>
            [JsonProperty("dateOfBirth", NullValueHandling = NullValueHandling.Ignore)]
            public string Dob { get; set; }

            /// <summary>
            /// Frankie: 
            /// </summary>
            [JsonProperty("yearOfBirth")]
            public string YearOfBirth { get; set; }

            public override string ToString()
            {
                return !string.IsNullOrWhiteSpace(Dob) 
                    ? Dob : YearOfBirth;
            }
        }

        public class Name
        {
            /// <summary>
            /// Frankie: 
            /// </summary>
            [JsonProperty("familyName")]
            public string FamilyName { get; set; }

            /// <summary>
            /// Frankie: 
            /// </summary>
            [JsonProperty("middleName")]
            public string MiddleName { get; set; }

            /// <summary>
            /// Frankie: 
            /// </summary>
            [JsonProperty("givenName")]
            public string GivenName { get; set; }

            public override string ToString()
            {
                return string.Join(" ", new [] { GivenName, MiddleName, FamilyName})
                    .Replace("  ", " ").Trim();
            }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Entity
        {
            [JsonProperty("name")]
            public Name Name { get; set; }

            [JsonProperty("dateOfBirth")]
            public DateOfBirth DateOfBirth { get; set; }
        }
        /// <summary>
        /// Frankie: Object to supply the country code, business name and number, along with an optional registry parameter to search for.
        /// </summary>

        public class EntityRequest
        {
            [JsonProperty("entity")]
            public Entity Entity { get; set; }
        }
    }
}
