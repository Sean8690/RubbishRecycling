using System.Net.Http;

namespace InfoTrack.Cdd.Application.Models
{
    /// <summary>
    /// Testable HttpResponseMessage and raw data string
    /// (Enables canned authority data to be easily used for testing purposes)
    /// </summary>
    public class TestableClientResponse
    {
        /// <summary>
        /// HttpResponseMessage
        /// </summary>
        public HttpResponseMessage ResponseMessage { get; set; }

        /// <summary>
        /// Deserialised HttpResponseMessage content string
        /// </summary>
        public string ResponseString { get; set; }
    }
}
