using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Storage.Contracts.Api.v2;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for building reports
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Build a pdf report
        /// </summary>
        Task<FileMetadata> GenerateReportPdfAsync<TModel>(TModel reportData, ServiceIdentifier serviceIdentifier, string fileName);
    }
}
