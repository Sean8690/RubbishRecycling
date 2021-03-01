using System;
using System.Threading;
using System.Threading.Tasks;
using InfoTrack.Orders.Api.Client;
using InfoTrack.Services.Api.Client;
using Microsoft.Extensions.Logging;
using OrdersProblemDetails = InfoTrack.Orders.Api.Client.ProblemDetails;
using ServicesProblemDetails = InfoTrack.Services.Api.Client.ProblemDetails;

namespace InfoTrack.Cdd.Infrastructure.Api.Platform
{
    /// <summary>
    /// Get organisation data from provider Frankie Financial
    /// </summary>
    public abstract class PlatformServiceBase
    {
        private readonly ILogger _logger;

        protected PlatformServiceBase(ILogger<PlatformServiceBase> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected internal async Task<TResponse> CallAuthority<TRequest, TResponse>(TRequest request,
            Func<TRequest, CancellationToken, Task<TResponse>> authorityEndpoint, CancellationToken cancellationToken)
        {
            if (authorityEndpoint == null)
            {
                throw new ArgumentNullException(nameof(authorityEndpoint));
            }

            try
            {
                return await authorityEndpoint(request, cancellationToken);
            }
            catch (OrdersApiClientException ex)
            {
                try
                {
                    var problemDetails = (ex as OrdersApiClientException<OrdersProblemDetails>)?.Result;
                    _logger.LogError("Non-success response code was returned when calling the Orders API: {StatusCode} {ReasonPhrase} {@ProblemDetails}", ex.StatusCode, ex.Message, problemDetails);
                }
                catch
                {
                    _logger.LogError("Non-success response code was returned when calling the Orders API: {StatusCode} {ReasonPhrase}", ex.StatusCode, ex.Message);
                }

                throw new InvalidOperationException($"Non-success response code was returned when calling the Orders API.", ex);
            }
            catch (ServicesApiClientException ex)
            {
                try
                {
                    var problemDetails = (ex as ServicesApiClientException<ServicesProblemDetails>)?.Result;
                    _logger.LogError("Non-success response code was returned when calling the Services API: {StatusCode} {ReasonPhrase} {@ProblemDetails}", ex.StatusCode, ex.Message, problemDetails);
                }
                catch
                {
                    _logger.LogError("Non-success response code was returned when calling the Services API: {StatusCode} {ReasonPhrase}", ex.StatusCode, ex.Message);
                }
                throw new InvalidOperationException($"Non-success response code was returned when calling the Services API.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error was thrown when calling a Platform.Core API");
                throw;
            }
        }

        protected internal async Task CallAuthority<TRequest1, TRequest2>(TRequest1 request1,
            TRequest2 request2, Func<TRequest1, TRequest2, CancellationToken, Task> authorityEndpoint, CancellationToken cancellationToken)
        {
            if (authorityEndpoint == null)
            {
                throw new ArgumentNullException(nameof(authorityEndpoint));
            }

            try
            {
                await authorityEndpoint(request1, request2, cancellationToken);
            }
            catch (OrdersApiClientException ex)
            {
                try
                {
                    var problemDetails = (ex as OrdersApiClientException<OrdersProblemDetails>)?.Result;
                    _logger.LogError("Non-success response code was returned when calling the Orders API: {StatusCode} {ReasonPhrase} {@ProblemDetails}", ex.StatusCode, ex.Message, problemDetails);
                }
                catch
                {
                    _logger.LogError("Non-success response code was returned when calling the Orders API: {StatusCode} {ReasonPhrase}", ex.StatusCode, ex.Message);
                }

                throw new InvalidOperationException($"Non-success response code was returned when calling the Orders API.", ex);
            }
            catch (ServicesApiClientException ex)
            {
                try
                {
                    var problemDetails = (ex as ServicesApiClientException<ServicesProblemDetails>)?.Result;
                    _logger.LogError("Non-success response code was returned when calling the Services API: {StatusCode} {ReasonPhrase} {@ProblemDetails}", ex.StatusCode, ex.Message, problemDetails);
                }
                catch
                {
                    _logger.LogError("Non-success response code was returned when calling the Services API: {StatusCode} {ReasonPhrase}", ex.StatusCode, ex.Message);
                }
                throw new InvalidOperationException($"Non-success response code was returned when calling the Services API.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error was thrown when calling a Platform.Core API");
                throw;
            }
        }

        protected internal async Task<TResponse> CallAuthority<TRequest1, TRequest2, TResponse>(TRequest1 request1,
                    TRequest2 request2, Func<TRequest1, TRequest2, CancellationToken, Task<TResponse>> authorityEndpoint, CancellationToken cancellationToken)
        {
            if (authorityEndpoint == null)
            {
                throw new ArgumentNullException(nameof(authorityEndpoint));
            }

            try
            {
                return await authorityEndpoint(request1, request2, cancellationToken);
            }
            catch (OrdersApiClientException ex)
            {
                try
                {
                    var problemDetails = (ex as OrdersApiClientException<OrdersProblemDetails>)?.Result;
                    _logger.LogError("Non-success response code was returned when calling the Orders API: {StatusCode} {ReasonPhrase} {@ProblemDetails}", ex.StatusCode, ex.Message, problemDetails);
                }
                catch
                {
                    _logger.LogError("Non-success response code was returned when calling the Orders API: {StatusCode} {ReasonPhrase}", ex.StatusCode, ex.Message);
                }

                throw new InvalidOperationException($"Non-success response code was returned when calling the Orders API.", ex);
            }
            catch (ServicesApiClientException ex)
            {
                try
                {
                    var problemDetails = (ex as ServicesApiClientException<ServicesProblemDetails>)?.Result;
                    _logger.LogError("Non-success response code was returned when calling the Services API: {StatusCode} {ReasonPhrase} {@ProblemDetails}", ex.StatusCode, ex.Message, problemDetails);
                }
                catch
                {
                    _logger.LogError("Non-success response code was returned when calling the Services API: {StatusCode} {ReasonPhrase}", ex.StatusCode, ex.Message);
                }
                throw new InvalidOperationException($"Non-success response code was returned when calling the Services API.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error was thrown when calling a Platform.Core API");
                throw;
            }
        }
    }
}
