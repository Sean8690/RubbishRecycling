using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
// ReSharper disable UnusedMember.Global

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie
{
    public class ErrorObject
    {
        /// <summary>
        /// Frankie: 	RequestIDObjectstring($ulid)
        /// example: 01BFJA617JMJXEW6G7TDDXNSHX
        /// Unique identifier for every request.Can be used for tracking down answers with technical support.
        /// Uses the ULID format (a time-based, sortable UUID)
        /// Note: this will be different for every request.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Frankie: HTTP status code. Same as that which is passed back in the header.
        /// </summary>
        [Obsolete("Frankie has marked this as obsolete")]
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// Frankie error code
        /// example: CORE-5990
        /// </summary>
        public string ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        /// <summary>
        /// Server version indication
        /// example: 2af478ed
        /// </summary>
        public string Commit { get; set; }

        public List<Issue> Issues { get; set; }
    }

    public class Issue
    {
        /// <summary>
        /// Will describe the field or data location of the issue
        /// example: dateOfBirth
        /// </summary>
        public string IssueLocation { get; set; }

        /// <summary>
        /// Description of the problem
        /// example: Invalid format. Must be YYYY-MM-DD
        /// </summary>
        [JsonProperty("Issue")]
        public string IssueDescription { get; set; }

    }
}
