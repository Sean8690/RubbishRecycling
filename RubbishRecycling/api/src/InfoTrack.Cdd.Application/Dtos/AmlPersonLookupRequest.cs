

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using InfoTrack.Cdd.Application.Common.Enums;
using Newtonsoft.Json.Converters;

namespace InfoTrack.Cdd.Application.Dtos
{

    /// <summary>
    /// Request for AML Lookup
    /// </summary>
    public class AmlPersonLookupRequest
    {
        /// <summary>
        /// First name of person to lookup for.
        /// </summary>
        [Required]
        public string GivenName { get; set; }

        /// <summary>
        /// Middle name of person to lookup for.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Last name of person to lookup for.
        /// </summary>
        [Required]
        public string FamilyName { get; set; }

        /// <summary>
        /// Date of Birth of the person to look for.
        /// </summary>
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Year of Birth of the person to look for.
        /// </summary>
        public string YearOfBirth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// Year of Birth of the person to look for.
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        /// Retailer reference for the client
        /// </summary>
        public string RetailerReference { get; set; }

        /// <summary>
        /// QuoteId
        /// </summary>
        public Guid? QuoteId { get; set; }
    }
}
