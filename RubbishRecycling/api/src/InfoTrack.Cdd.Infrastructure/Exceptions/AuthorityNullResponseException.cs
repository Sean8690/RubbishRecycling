using System;

namespace InfoTrack.Cdd.Infrastructure.Exceptions
{
    /// <summary>
    /// The authority returned a (successful) null or empty response
    /// </summary>
    public class AuthorityNullResponseException : AuthorityResponseException
    {
        public AuthorityNullResponseException(object response) : base(response) { }
        public AuthorityNullResponseException(object response, string message) : base(response, message) { }
        public AuthorityNullResponseException(object response, string message, string errorCode) : base(response, message, errorCode) { }
        public AuthorityNullResponseException(object response, string message, Exception innerException) : base(response, message, innerException) { }
    }
}
