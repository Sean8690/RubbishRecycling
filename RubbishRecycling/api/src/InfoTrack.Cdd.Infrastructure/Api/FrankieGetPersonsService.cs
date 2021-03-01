using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Cdd.Infrastructure.Exceptions;
using InfoTrack.Cdd.Infrastructure.Models.Frankie;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch;
using InfoTrack.Platform.Core.Contract.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch.PersonSearchCriteria;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions;
using InfoTrack.Cdd.Application.Common.Enums;
using System.Threading;
using InfoTrack.Storage.Contracts.Api.v2;
using InfoTrack.Cdd.Infrastructure.Constants;
using InfoTrack.Cdd.Application.Commands.PersonOrder;

namespace InfoTrack.Cdd.Infrastructure.Api
{
    /// <summary>
    /// 
    /// </summary>
    class FrankieGetPersonsService : AuthorityServiceBase, IPersonsService
    {
        private readonly ITestablePersonClient _client;
        private readonly IReportService _reportService;
        private readonly IOrderService _orderService;
        private readonly ILogger<FrankieGetPersonsService> _logger;

        public FrankieGetPersonsService(
            ITestablePersonClient client,
            IReportService reportService,
            IFileService fileService,
            IOrderService orderService,
            ILogger<FrankieGetPersonsService> logger) : base(fileService, logger)
        {
            _client = client;
            _reportService = reportService;
            _orderService = orderService;
            _logger = logger;
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

        public async Task<PersonMatches> GetPersonsAsync(string givenName, string middleName, string familyName, string dob, string yearOfBirth, int orderId)
        {
            var request = new EntityRequest
            {
                Entity = new Entity
                {
                    Name = new Name
                    {
                        GivenName = givenName,
                        FamilyName = familyName,
                        MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName,
                    },
                    DateOfBirth = (string.IsNullOrWhiteSpace(dob) && string.IsNullOrWhiteSpace(yearOfBirth)) ? null : new DateOfBirth
                    {
                        Dob = string.IsNullOrWhiteSpace(dob) ? null : dob,
                        YearOfBirth = string.IsNullOrWhiteSpace(yearOfBirth) ? null : yearOfBirth
                    }
                }
            };

            var response = await _client.GetPersonsAsync(request);

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
                    throw new AuthorityResponseException(errorResponse, errorResponse.ErrorMsg, errorResponse.ErrorCode);
                }
                else
                {
                    throw new AuthorityException($"Authority returned an unexpected status code.");
                }
            }

            var successResponse = JsonConvert.DeserializeObject<CheckEntityCheckResultObject>(response.ResponseString);
            // TODO:: Add more checks here
            if (successResponse?.RequestId == null)
            {
                throw new AuthorityNullResponseException(successResponse, "Authority returned an unexpected empty response.");
            }
            // Extract the response for UI here: 
            var amlPersonLookupTransformedResponse = successResponse.TransformAuthorityResponse(request);
            if (amlPersonLookupTransformedResponse == null)
            {
                throw new FormatException($"Failed to parse the authority response for orderId {orderId}");
            }

            var person = new PersonMatches
            {
                RawResponse = successResponse,
                SearchName = request.Entity?.Name?.ToString(),
                SearchDob = request.Entity?.DateOfBirth?.ToString(),
                Matches = GetLookupMatches(amlPersonLookupTransformedResponse)
            };

            return person;
        }

        private static List<PersonLite> GetLookupMatches(PersonSearchResponse data)
        {
            return data?.PersonResults
                .Where(p => p?.PersonDetails != null)
                .Select(p => p.PersonDetails)
                .OrderBy(p => p.AmlScore)
                    .ThenBy(p => p.FullName).ThenBy(p => p.Dob)
                .Select(p => new PersonLite
                {
                    Countries = p.Countries,
                    FullName = p.GetName(),
                    Dob = p.GetDob(),
                    ProviderEntityCode = p?.ProviderEntityCode
                }).ToList();
        }

        public async Task<Person> GetPersonReportAsync(int parentOrderId, ServiceIdentifier serviceIdentifier, string providerEntityCode, int childOrderId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(providerEntityCode))
            {
                throw new ArgumentNullException(nameof(providerEntityCode));
            }

            PersonSearchResponse parentData = await GetPersonSearchResponse(parentOrderId, cancellationToken);

            FrankieSearchResults childData;
            if (providerEntityCode == Defaults.NilResult)
            {
                childData = new FrankieSearchResults();
                childData.PersonDetails.ProviderEntityCode = providerEntityCode;
                childData.PersonDetails.SearchDetails = parentData.PersonResults
                    .FirstOrDefault()?.PersonDetails?.SearchDetails; // FirstOrDefault is safe because even if there were multiple results, the search query used for all of them is the same

                // get the parent request to know what we searched on
                // we need to update those props on the response
                var parentRequest = await _orderService.GetRequestAsync<PersonOrderRequestCommand>(parentOrderId, cancellationToken);
                childData.PersonDetails.SearchDetails.Name = string.Join(" ", new[] { parentRequest.GivenName, parentRequest.MiddleName, parentRequest.FamilyName })
                                                        .Replace("  ", " ").Trim();
                childData.PersonDetails.SearchDetails.YearOfBirth = parentRequest.YearOfBirth;
                childData.PersonDetails.SearchDetails.Dob = parentRequest.DateOfBirth;
            }
            else
            {
                // TODO I think it may be more complex than this. But also maybe not.
                childData = parentData.PersonResults.SingleOrDefault(p => p.PersonDetails?.ProviderEntityCode == providerEntityCode);
            }

            var fileName = childOrderId.ToString();
            var reportFile = await _reportService.GenerateReportPdfAsync<FrankieSearchResults>(childData, serviceIdentifier, fileName);

            var searchReport = childData.PersonDetails?.SearchUrl != null ? 
                await StoreAuthorityResponse(childData.PersonDetails.SearchUrl, "AuthoritySearch") : null;
            var detailReport = childData.PersonDetails?.ReportUrl != null ? 
                await StoreAuthorityResponse(childData.PersonDetails.ReportUrl, "AuthorityReport") : null;
            var authorityResponseFiles = (searchReport != null || detailReport != null) ? 
                new List<FileMetadata> { searchReport, detailReport }.Where(r => r != null).ToList() : null;
            
            return new Person
            {
                Name = childData.PersonDetails.GetName(),
                ProviderEntityCode = providerEntityCode,
                Reports = new List<FileMetadata> { reportFile },
                AuthorityResponseFiles = authorityResponseFiles
            };
        }

        private async Task<PersonSearchResponse> GetPersonSearchResponse(int parentOrderId, CancellationToken cancellationToken)
        {
            var response = await _orderService.GetResponseAsync<CheckEntityCheckResultObject>(parentOrderId, cancellationToken);

            var amlPersonLookupTransformedResponse = response.TransformAuthorityResponse(null);
            if (amlPersonLookupTransformedResponse == null)
            {
                throw new FormatException($"Failed to parse the authority response for orderId {parentOrderId}");
            }

            return amlPersonLookupTransformedResponse;
        }
    }
}
