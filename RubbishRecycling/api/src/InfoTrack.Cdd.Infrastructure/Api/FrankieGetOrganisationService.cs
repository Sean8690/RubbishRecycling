using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Cdd.Infrastructure.Exceptions;
using InfoTrack.Cdd.Infrastructure.Models.Frankie;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions;
using InfoTrack.Platform.Core.Contract.Exceptions;
using InfoTrack.Storage.Contracts.Api.v2;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InfoTrack.Cdd.Infrastructure.Api
{
    /// <summary>
    /// Get organisation data from provider Frankie Financial
    /// </summary>
    public class FrankieGetOrganisationService : AuthorityServiceBase, IOrganisationService
    {
        private readonly ITestableOrganisationClient _client;

        private readonly ICountryService _countryService;
        private readonly IReportService _reportService;
        private readonly ILogger<FrankieGetOrganisationService> _logger;

        public FrankieGetOrganisationService(
            ITestableOrganisationClient client,
            ICountryService countryService,
            IReportService reportService,
            IFileService fileService,
            ILogger<FrankieGetOrganisationService> logger) : base(fileService, logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> PingAsync()
        {
            var response = await _client.PingAsync();

            if (!response.ResponseMessage.IsSuccessStatusCode)
            {
                return false;
            }

            var responseObject = JsonConvert.DeserializeObject<PuppyObject>(response.ResponseString);
            return responseObject.Puppy;
        }

        public async Task<IEnumerable<OrganisationLite>> GetOrganisationsAsync(string name, string number, string kyckrCountryCode)
        {
            var request = new InternationalBusinessSearchCriteria { Name = name, Number = number, KyckrCountryCode = kyckrCountryCode };

            var response = await _client.GetOrganisationsAsync(request);

            if (!response.ResponseMessage.IsSuccessStatusCode)
            {
                ErrorObject errorResponse = null;
                try
                {
                    errorResponse = JsonConvert.DeserializeObject<ErrorObject>(response.ResponseString);

                    var isNoResults404 = response.ResponseMessage.StatusCode == HttpStatusCode.NotFound && errorResponse.ErrorCode == "SYS-0404";
                    if (isNoResults404)
                    {
                        _logger.LogWarning("Frankie API returned a {StatusCode} response with ErrorObject {@ErrorObject}",
                            response.ResponseMessage.StatusCode, errorResponse);
                        return new List<OrganisationLite>();
                    }
                    else
                    {
                        _logger.LogError("Frankie API returned non-success status code {StatusCode} and ErrorObject {@ErrorObject}",
                            response.ResponseMessage.StatusCode, errorResponse);
                    }
                }
                catch
                {
                    _logger.LogError("Frankie API returned non-success status code {StatusCode} and ErrorObject {ErrorResponseString}",
                        response.ResponseMessage.StatusCode, await response.ResponseMessage.Content.ReadAsStringAsync());
                }

                if (errorResponse != null)
                {
                    throw new AuthorityResponseException(errorResponse, errorResponse.ErrorMsg, errorResponse?.ErrorCode);
                }
                else
                {
                    throw new AuthorityException($"Authority returned a {response.ResponseMessage.StatusCode} status code.", errorResponse?.ErrorCode);
                }
            }

            var successResponse = JsonConvert.DeserializeObject<InternationalBusinessSearchResponse>(response.ResponseString);
            if (successResponse?.RequestId == null)
            {
                throw new AuthorityNullResponseException(successResponse, "Authority returned an unexpected empty response.");
            }

            return successResponse.Companies?
                .Where(c => c != null)
                .GroupBy(c => c.ProviderEntityCode) // these are sometimes duplicated
                .Select(c => new OrganisationLite
                {
                    Name = c.First().Name,
                    Number = c.First().CompanyNumber,
                    KyckrCountryCode = kyckrCountryCode,
                    ProviderEntityCode = c.First().ProviderEntityCode,
                    Address = c.First().Addresses.ToAddressString()
                });
        }

        public async Task<Organisation> GetOrganisationAsync(string providerEntityCode, string kyckrCountryCode, int orderId, ServiceIdentifier serviceIdentifier)
        {
            var request = new InternationalBusinessProfileCriteria { KyckrCountryCode = kyckrCountryCode, ProviderEntityCode = providerEntityCode };

            var response = await _client.GetOrganisationAsync(request);

            if (!response.ResponseMessage.IsSuccessStatusCode)
            {
                ErrorObject errorResponse = null;
                try
                {
                    errorResponse = JsonConvert.DeserializeObject<ErrorObject>(response.ResponseString);

                    _logger.LogError("Frankie API returned non-success status code {StatusCode} and ErrorObject {@ErrorObject}",
                        response.ResponseMessage.StatusCode, errorResponse);
                }
                catch
                {
                    _logger.LogError("Frankie API returned non-success status code {StatusCode} and ErrorObject {ErrorResponseString}",
                        response.ResponseMessage.StatusCode, await response.ResponseMessage.Content.ReadAsStringAsync());
                }

                if (errorResponse != null)
                {
                    var isUnavailable503 = response.ResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable && errorResponse.ErrorCode == "SYS-0503";
                    if (isUnavailable503)
                    {
                        throw new AuthorityCommunicationException(errorResponse.ErrorMsg);
                    }
                    throw new AuthorityResponseException(errorResponse, errorResponse.ErrorMsg, errorResponse?.ErrorCode);
                }
                else
                {
                    throw new AuthorityException($"Authority returned a {response.ResponseMessage.StatusCode} status code.", errorResponse?.ErrorCode);
                }
            }

            var successResponse = JsonConvert.DeserializeObject<InternationalBusinessProfileResponse>(response.ResponseString);
            if (successResponse?.RequestId == null || successResponse.CompanyProfile == null)
            {
                throw new AuthorityNullResponseException(successResponse, "Authority returned an unexpected empty response.");
            }

            await InfillCountry(successResponse, kyckrCountryCode);

            var fileName = orderId.ToString();
            var reportFile = await _reportService.GenerateReportPdfAsync(successResponse, 
                serviceIdentifier, fileName);

            var authorityResponseFile = successResponse.KyckrReport != null ?
                await StoreAuthorityResponse(successResponse.KyckrReport, "Kyckr") : null;

            return new Organisation
            {
                Name = successResponse.CompanyProfile.Name,
                Number = successResponse.CompanyProfile.CompanyNumber,
                KyckrCountryCode = kyckrCountryCode,
                ProviderEntityCode = successResponse.CompanyProfile.ProviderEntityCode ?? providerEntityCode,
                Address = successResponse.CompanyProfile.Addresses?.ToAddressString(),
                RawResponse = successResponse,
                Reports = new List<FileMetadata> { reportFile },
                AuthorityResponseFiles = authorityResponseFile != null ? new List<FileMetadata> { authorityResponseFile } : null
            };
        }

        /// <summary>
        /// Infill country details onto response
        /// </summary>
        private async Task InfillCountry(InternationalBusinessProfileResponse successResponse, string kyckrCountryCode)
        {
            if (string.IsNullOrEmpty(kyckrCountryCode))
            {
                return;
            }
            successResponse.KyckrCountryCode = kyckrCountryCode;
            var country = await _countryService.GetCountryAsync(kyckrCountryCode);
            if (country != null)
            {
                successResponse.CountryName = country.DisplayName;
                successResponse.CountryFlag = country.FlagUri;
            }
        }


    }
}
