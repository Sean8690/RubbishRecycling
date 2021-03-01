using System;
using System.IO;
using System.Threading.Tasks;
using InfoTrack.Storage.Contracts.Api.v2;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for manipulating files
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Store file from a stream
        /// </summary>
        Task<FileMetadata> StoreFileAsync(Stream stream, string fileName);

        /// <summary>
        /// Store a copy of a file from a uri
        /// </summary>
        Task<FileMetadata> StoreFileAsync(Uri uri, string fileName);
    }
}
