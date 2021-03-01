using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Common.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InfoTrack.Cdd.Application.Queries.SystemInfo
{
    /// <summary>
    /// Get basic system and region info
    /// </summary>
    public class GetSystemInfoQueryHandler : IQueryHandler<GetSystemInfoQuery, SystemInfoDto>
    {
        private readonly IConfiguration _config;
        private readonly ILogger<GetSystemInfoQueryHandler> _logger;

        /// <summary>
        /// Get basic system and region info
        /// </summary>
        public GetSystemInfoQueryHandler(IConfiguration config, ILogger<GetSystemInfoQueryHandler> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #pragma warning disable 1591
        public async Task<SystemInfoDto> Handle(GetSystemInfoQuery request, CancellationToken cancellationToken)
        #pragma warning restore 1591
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            
            var result = new SystemInfoDto
            {
                Name = assemblyName.Name,
                Version = assemblyName.Version.ToString(),
                Application = _config["Serilog:Properties:Application"],
                Environment = _config["Serilog:Properties:Environment"],
                Region = _config["Serilog:Properties:Region"],
                TimeZoneId = _config["InfoTrackApi:TimeZoneId"]
            };

            var privateResult = new PrivateSystemInfoDto
            {
                Authority = _config["AuthOptions:Authority"],
                Authentication = _config["Authentication:Authority"],
                FrankieUri = _config["FrankieApi:BaseUri"],
                FrankieCustomerId = _config["FrankieApi:CustomerId"],
                Mongo = _config["Mongo:ConnectionString"],
                PdfService = _config["PlatformServices:pdfServiceUrl"],
                TemplateService = _config["PlatformServices:templateServiceUrl"],
                StorageService = _config["PlatformServices:storageServiceUrl"],
                EmailService = _config["PlatformServices:emailServiceUrl"],
                OrdersApi = _config["OrdersApi:BaseUri"],
                ServicesApi = _config["ServicesApi:BaseUri"],
                PartiesApi = _config["PartiesApi:BaseUri"],
                OrderWaiterApi = _config["OrderWaiterApi:BaseUri"]
            };

            _logger.LogInformation("SYSTEM DIAGNOSTIC INFO: {@SystemInfoDto} and {@PrivateSystemInfoDto}", result, privateResult);

            return result;
        }
    }
}
