using System;
using System.Collections.Generic;
using InfoTrack.Cdd.Infrastructure.Utils;
using Newtonsoft.Json;

#pragma warning disable CA1002 // Do not expose generic lists

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared
{
    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class BackgroundCheckResultObject
    {
        /// <summary>
        /// This is the single identifier use to group results across all check types for a single check pass
        /// </summary>
        public string CheckId { get; set; }

        /// <summary>
        /// Indicator of what sort of results are in this BCRO.
        /// </summary>
        public BackgroundCheckType BackgroundCheckType { get; set; }

        /// <summary>
        /// RFC3399 formatted date-timestamp that the check was first verified.
        /// </summary>
        public DateTime FirstCheckDate { get; set; }

        /// <summary>
        /// RFC3399 formatted date-timestamp when this record was last checked/verified at the source.
        /// </summary>
        public DateTime LatestCheckDate { get; set; }

        /// <summary>
        /// Current state, based on the most recent check.
        /// </summary>
        public BackgroundCheckState CurrentState { get; set; }

        /// <summary>
        /// Service provider that performed the check.
        /// example: "acuris"
        /// </summary>
        public string CheckPerformedBy { get; set; }

        /// <summary>
        /// Where available, the source name of the data. This might be something like
        /// “pep-class-1” or “dfat_consolidated_list” or other list source.
        /// </summary>
        public string CheckSource { get; set; }

        /// <summary>
        /// Usually, the service provider will give us a receipt, transaction id, check number, or some
        /// such that gives us a unique id on their side that we can reconcile with. This value will be
        /// returned unless there is an error.
        /// </summary>
        [JsonProperty("providerCheckID")]
        public string ProviderCheckId { get; set; }

        /// <summary>
        /// Loosely typed Key-Value-Pairs (KVP) to store notes and details around the result.
        /// See Key Value Pairs documentation for more information.
        /// This is where the bulk of the result data will be found. We'll describe these in more detail in the AML Result Set Elements page.
        /// kvpType should generally be set to “general.string”
        /// </summary>
        [JsonProperty("bcro"), JsonWrappedObjectList("checkDetails")]
        public List<KeyValuePairs> CheckDetails { get; set; }
    }

    public class KeyValuePairs
    {
        public string KvpKey { get; set; }
        public string KvpType { get; set; }
        public string KvpValue { get; set; }
    }
}
