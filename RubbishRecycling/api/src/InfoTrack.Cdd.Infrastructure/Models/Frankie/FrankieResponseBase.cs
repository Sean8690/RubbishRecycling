using Newtonsoft.Json;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie
{
    public abstract class FrankieResponseBase
    {
        /// <summary>
        /// Frankie: Service provider ID (I think this is Kyckr's ID)
        /// </summary>
        [JsonProperty("ibTransactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Frankie: Service provider response code  (I think this is Kyckr's response code)
        /// </summary>
        [JsonProperty("ibResponseCode")]
        public string ResponseCode { get; set; }

        /// <summary>
        /// Null in examples provided so far. This may be intended to be Kyckr's raw response
        /// </summary>
        [JsonProperty("ibResponseDetails")]
        public string ResponseDetails { get; set; }
    }
}
