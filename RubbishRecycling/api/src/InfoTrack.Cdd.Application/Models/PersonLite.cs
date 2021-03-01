using System;
using System.Collections.Generic;
using InfoTrack.Cdd.Application.Common.Interfaces;

namespace InfoTrack.Cdd.Application.Models
{
    /// <summary>
    /// Basic person info.
    ///
    /// More info is available from the provider if required, but the structure of this object has been kept simple for now
    /// </summary>
    public class PersonLite : IPerson
    {
        /// <summary>
        /// Provider's unique entity identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Date of Birth of the person to look for.
        /// </summary>
        public string Dob { get; set; }

        /// <summary>
        /// Date of Birth of the person to look for.
        /// </summary>
        public string YearOfBirth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Countries { get; set; }
    }
}
