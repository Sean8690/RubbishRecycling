namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch
{
    public interface IUniqueReportingDetail
    {
        /// <summary>
        /// Like a matter reference for this search (which may have returned multiple entities)
        /// </summary>
        string CaseId { get; set; }

        /// <summary>
        /// Refers to a single entity returned for the current case/search
        /// </summary>
        string ProviderEntityCode { get; set; }

        /// <summary>
        /// (I think) refers to a specific instance of a search (e.g. the search for this entity performed on this date)
        /// </summary>
        string ReferenceId { get; set; }

        /// <summary>
        /// CaseId::SearchEntityId(ProviderEntityCode)::SearchReferenceId
        /// </summary>
        string GroupingId { get; set; }
    }
}
