using System;
using System.Collections.Generic;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared;
using InfoTrack.Cdd.Infrastructure.Utils;
using Newtonsoft.Json;
using static InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch.PersonSearchResponse;

#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch
{
    /// <summary>
    /// Describes all of the checks that were carried out against an entity as part of our cascading check process.Because there are a number of steps involved in checking an entity, (including the use of past checks done by you or others), there is an overall summary check result that will tell you the final disposition of the the check you requested.
    /// So if you requested a 2+2+governmentID+pep/sanctions/etc (i.e.everything) then there would have been several checks done in order to meet this requirement.Some may have even failed, but eventually we got there.The summary gives the final assessment, based on all available data.
    /// Detailed writeups on how this all works can be found here
    /// https://apidocs.frankiefinancial.com/docs/understanding-checksummary-results
    /// </summary>
    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class CheckEntityCheckResultObject
    {
        /// <summary>
        /// Frankie: 	RequestIDObjectstring($ulid)
        /// example: 01BFJA617JMJXEW6G7TDDXNSHX
        /// Unique identifier for every request.Can be used for tracking down answers with technical support.
        /// Uses the ULID format (a time-based, sortable UUID)
        /// Note: this will be different for every request.
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("entityResult")]
        public EntityResult EntityResults { get; set; }
    }

    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class EntityResult
    {
        /// <summary>
        /// Each item represents a single matched person of interest
        /// </summary>
        [JsonProperty("amlResultSets")]
        public List<AmlResults> AmlResultsSet { get; set; }
    }

    /// <summary>
    /// A single matched person of interest
    /// </summary>
    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class AmlResults
    {
        /// <summary>
        /// This is a summary BackgroundCheckResultObject (BCRO) that provides details of the search
        /// criteria matched, aliases, associates and other summary data
        /// </summary>
        [JsonProperty("groupDetails")]
        public BackgroundCheckResultObject GroupDetails { get; set; }

        /// <summary>
        /// This is an array of (BCRO)'s - each one detailing a separate Politically Exposed Person (PEP)
        /// result for the matched person.
        /// If a person holds or has held 3 different political positions, then there will be 3 BCROs
        /// </summary>
        [JsonProperty("checkResultsListPEP")]
        public List<BackgroundCheckResultObject> CheckResultsListPEPs { get; set; }

        /// <summary>
        /// This is an array of (BCRO)'s - each one detailing a separate Sanction List
        /// result for the matched person.
        /// If a person is on 5 different sanctions lists, then there will be 5 BCROs
        /// </summary>
        [JsonProperty("checkResultsListSanctions")]
        public List<BackgroundCheckResultObject> CheckResultsListSanctions { get; set; }

        /// <summary>
        /// This is an array of (BCRO)'s - that record all matching adverse media articles. If there are
        /// media results, then there will be 2 or more media BCROs
        /// One will contain a collection of all media links
        /// One or more will contain summary data related to the media matches.
        /// </summary>
        [JsonProperty("checkResultsListMedia")]
        public List<BackgroundCheckResultObject> CheckResultsListMedias { get; set; }

        /// <summary>
        /// This is an array of (BCRO)'s - each one detailing a separate Watchlist result for the matched
        /// person.
        /// A "watchlist" is a catch-all term used to cover lists like:
        /// Non-sanction criminal wanted lists
        /// Corporate action list(e.g.disqualified directors)
        /// Fitness and probity lists
        /// As above, if a person is on 5 different watchlists, then there will be 5 BCROs
        /// </summary>
        [JsonProperty("checkResultsListWatchlists")]
        public List<BackgroundCheckResultObject> CheckResultsListWatchlists { get; set; }
    }

}
