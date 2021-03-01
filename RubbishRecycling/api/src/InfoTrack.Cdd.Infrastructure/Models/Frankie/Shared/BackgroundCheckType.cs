namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared
{
    public enum BackgroundCheckType
    {
        GROUP,
        PEP,
        SANCTION,
        WATCHLIST,
        MEDIA
    }

    public enum BackgroundCheckState
    {
        /// <summary>
        /// The no checks have ever turned up results
        /// </summary>
        CLEAR,
        /// <summary>
        /// Past checks have returned hits, but now they're clear. Only really relevant if we have a history of these checks.
        /// </summary>
        PAST_HITS,
        /// <summary>
        /// The most recent checks turned up some results that may be relevant (confidence <80)
        /// </summary>
        POSSIBLE_HIT,
        /// <summary>
        /// The current checks are returning definitive hits (confidence >= 80)
        /// </summary>
        ACTIVE_HITS
    }


}
