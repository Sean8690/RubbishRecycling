using System.Collections.Generic;
using InfoTrack.Storage.Contracts.Api.v2;

namespace InfoTrack.Cdd.Application.Models
{
    public interface IFrankieResponse
    {
        /// <summary>
        /// Identifying name (e.g. organisation name or person name)
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Authority response
        /// </summary>
        object RawResponse { get; set; }

        /// <summary>
        /// Order files
        /// </summary>
        List<FileMetadata> Reports { get; set; }

        /// <summary>
        /// Authority response files (hidden)
        /// </summary>
        List<FileMetadata> AuthorityResponseFiles { get; set; }
    }

    /// <summary>
    /// Detailed organisation info
    /// </summary>
    public class Organisation : OrganisationLite, IFrankieResponse
    {
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
