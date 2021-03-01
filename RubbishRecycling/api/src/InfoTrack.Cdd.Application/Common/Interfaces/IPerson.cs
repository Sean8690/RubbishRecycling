using System;
using System.Collections.Generic;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Basic person info Interface
    /// </summary>
    public interface IPerson
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
