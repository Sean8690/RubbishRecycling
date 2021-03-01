using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Storage.Contracts.Api;
using InfoTrack.Storage.Contracts.Api.v2;

namespace InfoTrack.Cdd.Infrastructure.Service
{
    public class FileService : IFileService
    {
        private readonly IStorageApi _storageApiClient;
        
        public FileService(IStorageApi storageApiClient)
        {
            _storageApiClient = storageApiClient ?? throw new ArgumentNullException(nameof(storageApiClient));
        }

        /// <summary>
        /// Store a file from a stream
        /// </summary>
        public async Task<FileMetadata> StoreFileAsync(Stream stream, string fileName)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (stream.CanSeek)
            {
                stream.Position = 0; // Just in case stream has already been iterated
            }

            var result = await _storageApiClient.PutFileAsync(fileName, stream);
            if (result.ContentLength <= 0 && stream.CanSeek)
            {
                stream.Position = 0;
                var length = stream.Length;

                return new FileMetadata(result.Name, result.RetrievalUrl, result.LastModified, result.ContentType, length, result.ETag, result.Metadata, result.UncPath);
            }
            return result;
        }

        public async Task<FileMetadata> StoreFileAsync(Uri uri, string fileName)
        {
            var fileStream = await DownloadFileAsync(uri);
            return await StoreFileAsync(fileStream, fileName);
        }

        private static async Task<Stream> DownloadFileAsync(Uri uri)
        {
            var request = WebRequest.Create(uri);
            var response = await request.GetResponseAsync();
            return response.GetResponseStream();
        }
    }
}
