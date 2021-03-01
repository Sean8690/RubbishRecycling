using System.Collections.Generic;
using InfoTrack.Storage.Contracts.Api.v2;

namespace InfoTrack.Cdd.Application.Models
{
    /// <summary>
    /// Detailed person info
    /// </summary>
    public class Person : IFrankieResponse
    {
        /// <summary>
        /// Identifying name (e.g. organisation name or person name)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Provider's unique organisation identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// Authority response
        /// </summary>
        public object RawResponse { get; set; }

        /// <summary>
        /// Order files
        /// </summary>
        public List<FileMetadata> Reports { get; set; } = new List<FileMetadata>();

        /// <summary>
        /// Authority response files (hidden)
        /// </summary>
        public List<FileMetadata> AuthorityResponseFiles { get; set; } = new List<FileMetadata>();
        
    }
}
