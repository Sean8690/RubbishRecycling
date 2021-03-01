using System;
using System.Linq;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Storage.Contracts.Api.v2;
using Microsoft.Extensions.Logging;

namespace InfoTrack.Cdd.Infrastructure.Api
{
    public abstract class AuthorityServiceBase
    {
        private readonly IFileService _fileService;
        private readonly ILogger _logger;

        protected internal AuthorityServiceBase(IFileService fileService, ILogger logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        protected internal async Task<FileMetadata> StoreAuthorityResponse(Uri url, string fileNamePrefix)
        {
            if (url != null)
            {
                try
                {
                    return await _fileService.StoreFileAsync(url, $"{fileNamePrefix}_{url.Segments.LastOrDefault()}.html");
                }
                #pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                #pragma warning restore CA1031 // Do not catch general exception types
                {
                    // Catch, log and hide ANY exception: this isn't important enough to interrupt control flow or fail the order
                    _logger.LogError(ex, "Unable to download authority response file {AuthorityResponseFile}", url?.ToString());
                }
            }

            return null;
        }
    }
}
