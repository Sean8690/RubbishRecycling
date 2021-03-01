using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfoTrack.Cdd.Application.Commands.Order;
using InfoTrack.Cdd.Application.Commands.PersonOrder;
using InfoTrack.Cdd.Application.Commands.PersonReport;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Cdd.Application.Dtos.Order;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Cdd.Infrastructure.Constants;
using InfoTrack.Cdd.Infrastructure.Exceptions;
using InfoTrack.Cdd.Infrastructure.Models;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch;
using InfoTrack.Platform.Core.Contract.Exceptions;
using Microsoft.Extensions.Logging;

namespace InfoTrack.Cdd.Infrastructure.Api.Platform
{
    /// <summary>
    /// Order service facade
    /// </summary>
    public class ServiceFacade : IServiceFacade
    {
        private readonly IOrderService _orderService;
        private readonly IOrganisationService _organisationService;
        private readonly IPersonsService _personService;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceFacade> _logger;

        public ServiceFacade(IOrderService orderService,
            IOrganisationService organisationService,
            IPersonsService personsService,
            IMapper mapper,
            ILogger<ServiceFacade> logger)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _organisationService = organisationService ?? throw new ArgumentNullException(nameof(organisationService));
            _personService = personsService ?? throw new ArgumentNullException(nameof(personsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Organisation

        /// <summary>
        /// Order detailed organisation info (report)
        /// </summary>
        public async Task<OrganisationSummaryResponseDto> OrderOrganisationSummaryAsync(OrganisationOrderRequestCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Create platform order
            var orderId = (await _orderService.OrderAsync(request, cancellationToken)).OrderId;

            // Call the authority and update the platform order (includes generating the pdf and setting the order to complete or errored)
            var organisationDetails = await AttachAuthorityResponseToOrderAsync(request, orderId, cancellationToken);

            // Fetch the order again, so we know that what we are returning is the correct order state as it exists now
            var order = await _orderService.GetAsync<OrganisationOrderRequestCommand>(orderId, cancellationToken);

            var response = _mapper.Map<OrganisationSummaryResponseDto>(order);
            if (organisationDetails != null)
            {
                response.Name = organisationDetails.Name;
                response.Number = organisationDetails.Number;
                response.KyckrCountryCode = organisationDetails.KyckrCountryCode;
                response.ProviderEntityCode = organisationDetails.ProviderEntityCode;
            }
            else
            {
                response.ProviderEntityCode = request.ProviderEntityCode;
                response.KyckrCountryCode = request.KyckrCountryCode;
            }

            return response;
        }

        /// <summary>
        /// TODO this should reeeeeaaaaaaaaallllly be generic like this but running out of time dammit
        /// </summary>
        private async Task<IFrankieResponse> AttachAuthorityResponseToOrderAsync(IFrankieResponse frankieResponse, int orderId, CancellationToken cancellationToken)
        {
            try
            {
                // Attach authority response and report to the order, and update order description
                var tasks = new List<Task>
                {
                    _orderService.SetResponseAsync(orderId, frankieResponse.RawResponse, cancellationToken),
                    _orderService.UpdateDescriptionAsync(orderId, frankieResponse.Name, cancellationToken)
                };
                tasks.AddRange(frankieResponse.Reports?
                    .Select(r => _orderService.AttachFileAsync(orderId, r, cancellationToken)));

                tasks.AddRange(frankieResponse.AuthorityResponseFiles?
                    .Select(r => _orderService.TryAttachAuthorityResponseFileAsync(orderId, r, cancellationToken)));

                await Task.WhenAll(tasks);

                // Finally, complete the order (do this last, because once completed the order will be sent to clients)
                await _orderService.CompleteAsync(orderId, null, cancellationToken);

                return frankieResponse;
            }
            catch (AuthorityCommunicationException ex)
            {
                await _orderService.ErrorAsync(orderId, ex.Message, cancellationToken);
                _logger.LogError(ex, "Unable to contact authority.");
            }
            catch (AuthorityNullResponseException ex)
            {
                await Task.WhenAll(new List<Task>
                {
                    _orderService.SetResponseAsync(orderId, ex.Response, cancellationToken),
                    _orderService.ErrorAsync(orderId, ex.Message, cancellationToken)
                });
                _logger.LogError(ex, "No records found (AuthorityResponse {@AuthorityResponse}).", ex.Response);
            }
            catch (AuthorityResponseException ex)
            {
                await Task.WhenAll(new List<Task>
                {
                    _orderService.SetResponseAsync(orderId, ex.Response, cancellationToken),
                    _orderService.ErrorAsync(orderId, ex.Message, cancellationToken)
                });
                _logger.LogError(ex, $"{nameof(AuthorityResponseException)} (AuthorityResponse {{@AuthorityResponse}}).", ex.Response);
            }
            catch (AuthorityException ex)
            {
                await Task.WhenAll(new List<Task>
                {
                    _orderService.ErrorAsync(orderId, ex.Message, cancellationToken)
                });
                _logger.LogError(ex, ex.GetType().Name);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex) // Warning disabled: we WANT to catch absolutely anything here
#pragma warning restore CA1031 // Do not catch general exception types
            {
                await _orderService.ErrorAsync(orderId, null, cancellationToken);
                _logger.LogError(ex, ex.GetType().Name);
            }

            return null;
        }

        private async Task<Organisation> AttachAuthorityResponseToOrderAsync(OrganisationOrderRequestCommand request, int orderId, CancellationToken cancellationToken)
        {
            try
            {
                // Get the organisation data from the authority (Frankie)
                // This will also generate the pdf report
                var frankieResponse = await _organisationService.GetOrganisationAsync(request.ProviderEntityCode, request.KyckrCountryCode, orderId, request.ServiceIdentifier);

                // Attach authority response and report to the order, and update order description
                var tasks = new List<Task>
                {
                    _orderService.SetResponseAsync(orderId, frankieResponse.RawResponse, cancellationToken),
                    _orderService.UpdateDescriptionAsync(orderId, frankieResponse.Name, cancellationToken)
                };
                tasks.AddRange(frankieResponse.Reports?
                    .Select(r => _orderService.AttachFileAsync(orderId, r, cancellationToken)));

                tasks.AddRange(frankieResponse.AuthorityResponseFiles?
                    .Select(r => _orderService.TryAttachAuthorityResponseFileAsync(orderId, r, cancellationToken)));

                await Task.WhenAll(tasks);

                // Finally, complete the order (do this last, because once completed the order will be sent to clients)
                await _orderService.CompleteAsync(orderId, null, cancellationToken);

                return frankieResponse;
            }
            catch (AuthorityCommunicationException ex)
            {
                await _orderService.ErrorAsync(orderId, ex.Message, cancellationToken);
                _logger.LogError(ex, "Unable to contact authority.");
            }
            catch (AuthorityNullResponseException ex)
            {
                await Task.WhenAll(new List<Task>
                {
                    _orderService.SetResponseAsync(orderId, ex.Response, cancellationToken),
                    _orderService.ErrorAsync(orderId, ex.Message, cancellationToken)
                });
                _logger.LogError(ex, "No records found (AuthorityResponse {@AuthorityResponse}).", ex.Response);
            }
            catch (AuthorityResponseException ex)
            {
                await Task.WhenAll(new List<Task>
                {
                    _orderService.SetResponseAsync(orderId, ex.Response, cancellationToken),
                    _orderService.ErrorAsync(orderId, ex.Message, cancellationToken)
                });
                _logger.LogError(ex, $"{nameof(AuthorityResponseException)} (AuthorityResponse {{@AuthorityResponse}}).", ex.Response);
            }
            catch (AuthorityException ex)
            {
                await Task.WhenAll(new List<Task>
                {
                    _orderService.ErrorAsync(orderId, ex.Message, cancellationToken)
                });
                _logger.LogError(ex, ex.GetType().Name);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex) // Warning disabled: we WANT to catch absolutely anything here
#pragma warning restore CA1031 // Do not catch general exception types
            {
                await _orderService.ErrorAsync(orderId, null, cancellationToken);
                _logger.LogError(ex, ex.GetType().Name);
            }

            return null;
        }

        #endregion

        #region Person

        /// <summary>
        /// Order detailed person info (report)
        /// </summary>
        public async Task<PersonListResponseDto> OrderPersonSummaryAsync(PersonOrderRequestCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Create platform parent order
            var orderId = (await _orderService.OrderAsync(request, cancellationToken, Orders.Api.Client.ApplicationStatusEnum.List)).OrderId;

            // Call the authority and update the platform order (includes generating the pdf and setting the order to complete or error-ed)
            var personDetails = await AttachAuthorityResponseToOrderAsync(request, orderId, cancellationToken);

            // Fetch the order again, so we know that what we are returning is the correct order state as it exists now
            var order = await _orderService.GetAsync<PersonOrderRequestCommand>(orderId, cancellationToken);

            var response = _mapper.Map<PersonListResponseDto>(order);
            if (response != null)
            {
                if (personDetails.Matches.Any())
                {
                    response.Matches.AddRange(personDetails.Matches);
                }
                else
                {
                    // If there were no matches, return a placeholder match
                    response.Matches.Add(new PersonLite
                    {
                        FullName = string.Join(" ", new[] { request.GivenName, request.MiddleName, request.FamilyName })
                            .Replace("  ", " ").Trim(),
                        Dob = !string.IsNullOrWhiteSpace(request.DateOfBirth) ? request.DateOfBirth : request.YearOfBirth,
                        ProviderEntityCode = Defaults.NilResult
                    });
                }
            }

            return response;
        }

        private async Task<PersonMatches> AttachAuthorityResponseToOrderAsync(PersonOrderRequestCommand request, int orderId, CancellationToken cancellationToken)
        {
            try
            {
                // Get the person data from the authority (Frankie)
                var frankieResponse = await _personService.GetPersonsAsync(request.GivenName, request.MiddleName, request.FamilyName, request.DateOfBirth, request.YearOfBirth, orderId);

                // Attach authority response, and update order description
                var tasks = new List<Task>
                    {
                        _orderService.SetResponseAsync(orderId, frankieResponse.RawResponse, cancellationToken),
                        _orderService.UpdateDescriptionAsync(orderId, frankieResponse.SearchName, cancellationToken)
                    };
                await Task.WhenAll(tasks);

                // List orders do not need to be completed

                return frankieResponse;
            }
            catch (AuthorityCommunicationException ex)
            {
                await _orderService.ErrorAsync(orderId, ex.Message, cancellationToken);
                _logger.LogError(ex, "Unable to contact authority.");
            }
            catch (AuthorityNullResponseException ex)
            {
                await Task.WhenAll(new List<Task>
                    {
                        _orderService.SetResponseAsync(orderId, ex.Response, cancellationToken),
                        _orderService.ErrorAsync(orderId, ex.Message, cancellationToken)
                    });
                _logger.LogError(ex, "No records found (AuthorityResponse {@AuthorityResponse}).", ex.Response);
            }
            catch (AuthorityResponseException ex)
            {
                await Task.WhenAll(new List<Task>
                    {
                        _orderService.SetResponseAsync(orderId, ex.Response, cancellationToken),
                        _orderService.ErrorAsync(orderId, ex.Message, cancellationToken)
                    });
                _logger.LogError(ex, $"{nameof(AuthorityResponseException)} (AuthorityResponse {{@AuthorityResponse}}).", ex.Response);
            }
            catch (AuthorityException ex)
            {
                await Task.WhenAll(new List<Task>
                    {
                        _orderService.ErrorAsync(orderId, ex.Message, cancellationToken)
                    });
                _logger.LogError(ex, ex.GetType().Name);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex) // Warning disabled: we WANT to catch absolutely anything here
#pragma warning restore CA1031 // Do not catch general exception types
            {
                await _orderService.ErrorAsync(orderId, null, cancellationToken);
                _logger.LogError(ex, ex.GetType().Name);
            }

            return null;
        }

        public async Task<List<OrderDto>> OrderPersonReportsAsync(PersonOrderReportCommand request, CancellationToken cancellationToken)
        {
            if (request?.OrderId == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Get a list of order requests, one for each ProviderEntityCode
            Dictionary<string, OrderRequest> childOrderRequestMap = request.ProviderEntityCodes
                .ToDictionary(k => k, v => _mapper.Map<OrderRequest>(request));
            foreach (var requests in childOrderRequestMap)
            {
                requests.Value.ProviderEntityCode = requests.Key;
            }
            var childOrderRequests = childOrderRequestMap.Select(o => o.Value);

            // Create platform child orders
            var results = await _orderService.OrderAsync(childOrderRequests, cancellationToken);

            // Call the authority and update the platform order (includes generating the pdf and setting the order to complete or error-ed)
            int parentOrderId = request.OrderId.Value;
            foreach (var c in results)
            {
                var x = await AttachAuthorityResponseToOrderAsync(parentOrderId, c.ProviderEntityCode, c.OrderId, cancellationToken);
            }

            // Fetch the order again, so we know that what we are returning is the correct order state as it exists now
            // TODO batch this more efficiently
            var orders = new List<OrderDto>();
            foreach (var o in results)
            {
                var platformOrder = await _orderService.GetAsync<PersonOrderReportCommand>(o.OrderId, cancellationToken);
                orders.Add(_mapper.Map<OrderDto>(platformOrder));
            }
            return orders;
        }

        /// <summary>
        /// TODO nooooooo copy paste bad bad bad
        /// </summary>
        private async Task<Person> AttachAuthorityResponseToOrderAsync(int parentOrderId, string providerEntityCode, int childOrderId, CancellationToken cancellationToken)
        {
            try
            {
                Person frankieResponse = await _personService.GetPersonReportAsync(parentOrderId, ServiceIdentifier.CddPersonRiskReport, providerEntityCode, childOrderId, cancellationToken);

                // Attach authority response, and update order description
                var tasks = new List<Task>
                    {
                        _orderService.UpdateDescriptionAsync(childOrderId, frankieResponse.Name, cancellationToken)
                    };

                if (frankieResponse.Reports != null)
                {
                    tasks.AddRange(frankieResponse.Reports
                    .Select(r => _orderService.AttachFileAsync(childOrderId, r, cancellationToken)));
                }

                if (frankieResponse.AuthorityResponseFiles != null)
                {
                    tasks.AddRange(frankieResponse.AuthorityResponseFiles
                    .Select(r => _orderService.TryAttachAuthorityResponseFileAsync(childOrderId, r, cancellationToken)));
                }

                await Task.WhenAll(tasks);

                // Finally, complete the order (do this last, because once completed the order will be sent to clients)
                await _orderService.CompleteAsync(childOrderId, null, cancellationToken);

                return frankieResponse;
            }
            catch (AuthorityCommunicationException ex)
            {
                await _orderService.ErrorAsync(childOrderId, ex.Message, cancellationToken);
                _logger.LogError(ex, "Unable to contact authority.");
            }
            catch (AuthorityNullResponseException ex)
            {
                await Task.WhenAll(new List<Task>
                    {
                        _orderService.SetResponseAsync(childOrderId, ex.Response, cancellationToken),
                        _orderService.ErrorAsync(childOrderId, ex.Message, cancellationToken)
                    });
                _logger.LogError(ex, "No records found (AuthorityResponse {@AuthorityResponse}).", ex.Response);
            }
            catch (AuthorityResponseException ex)
            {
                await Task.WhenAll(new List<Task>
                    {
                        _orderService.SetResponseAsync(childOrderId, ex.Response, cancellationToken),
                        _orderService.ErrorAsync(childOrderId, ex.Message, cancellationToken)
                    });
                _logger.LogError(ex, $"{nameof(AuthorityResponseException)} (AuthorityResponse {{@AuthorityResponse}}).", ex.Response);
            }
            catch (AuthorityException ex)
            {
                await Task.WhenAll(new List<Task>
                    {
                        _orderService.ErrorAsync(childOrderId, ex.Message, cancellationToken)
                    });
                _logger.LogError(ex, ex.GetType().Name);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex) // Warning disabled: we WANT to catch absolutely anything here
#pragma warning restore CA1031 // Do not catch general exception types
            {
                await _orderService.ErrorAsync(childOrderId, null, cancellationToken);
                _logger.LogError(ex, ex.GetType().Name);
            }

            return null;
        }

        #endregion
    }
}
