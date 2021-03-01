using System;
using InfoTrack.Platform.Core.Contract.Exceptions;

namespace InfoTrack.Cdd.Infrastructure.Exceptions
{
    /// <summary>
    /// The authority returned an error response
    /// </summary>
    public class AuthorityResponseException : AuthorityException
    {
        public object Response { get; }

        public AuthorityResponseException(object response) : base()
        {
            Response = response;
        }

        public AuthorityResponseException(object response, string message) : base(message)
        {
            Response = response;
        }

        public AuthorityResponseException(object response, string message, string errorCode) : base(message, errorCode)
        {
            Response = response;
        }

        public AuthorityResponseException(object response, string message, Exception innerException) : base(message, innerException)
        {
            Response = response;
        }
    }
}
