using System;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Cdd.Infrastructure.Config;
using InfoTrack.Cdd.Infrastructure.Utils;
using InfoTrack.PdfService.Client;
using InfoTrack.PdfService.Model;
using InfoTrack.Storage.Contracts.Api.v2;

namespace InfoTrack.Cdd.Infrastructure.Service
{
    public class ReportService : IReportService
    {
        private readonly PlatformServicesConfig _platformConfig;
        private readonly ITemplateBuilder _templateBuilder;
        private readonly IFileService _fileService;

        public ReportService(PlatformServicesConfig platformConfig, 
            ITemplateBuilder templateBuilder,
            IFileService fileService)
        {
            _platformConfig = platformConfig ?? throw new ArgumentNullException(nameof(platformConfig));
            _templateBuilder = templateBuilder ?? throw new ArgumentNullException(nameof(templateBuilder));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        /// <summary>
        /// Build a pdf report
        /// </summary>
        public async Task<FileMetadata> GenerateReportPdfAsync<TModel>(TModel reportData, ServiceIdentifier serviceIdentifier, string fileName)
        {
            var model = new TemplateModel<TModel>(reportData) { ReportTitle = serviceIdentifier.Description() };
            var htmlReport = await _templateBuilder.Build(model, serviceIdentifier.ToString());
            using var builder = new PdfBuilder(_platformConfig.PdfServiceUrl);
            builder.AddHtml(htmlReport);
            
            var options = new PdfOptions
            {
                FontEmbed = true,
                A4PageSize = true,
                AddPageNumbers = true,
                AddLinks = true,
                AddInternalPageLinks = true,
                ForcePortrait = true,
                ShowWithZeroMargins = true,
                FooterBoxHeight = 5
            };

            if (In8nHelper.DocumentPageSize == DocumentPageSize.Letter)
            {
                options.A4PageSize = false;
            }

            await using var stream = await builder.ToPdfAsync(options);
            return await _fileService.StoreFileAsync(stream, $"{fileName}.pdf");
        }
    }
}
