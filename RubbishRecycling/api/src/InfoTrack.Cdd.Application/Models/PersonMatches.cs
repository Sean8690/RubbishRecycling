using System;
using System.Collections.Generic;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Storage.Contracts.Api.v2;

namespace InfoTrack.Cdd.Application.Models
{
    /// <summary>
    /// Detailed person info
    /// </summary>
    public class PersonMatches
    {
        /// <summary>
        /// Authority response
        /// </summary>
        public object RawResponse { get; set; }

        /// <summary>
        /// Authority response
        /// </summary>
        public string SearchName { get; set; }

        public string SearchDob { get; set; }

        /// <summary>
        /// Authority response
        /// </summary>
        public List<PersonLite> Matches = new List<PersonLite>();
    }
}
