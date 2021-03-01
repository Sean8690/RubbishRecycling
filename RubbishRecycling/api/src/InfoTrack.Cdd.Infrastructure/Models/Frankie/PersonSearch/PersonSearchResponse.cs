using System;
using System.Collections.Generic;

#pragma warning disable CA1002 // Do not expose generic lists

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch
{
    /// <summary>
    /// Represents multiple matched persons of interest
    /// </summary>
    public class PersonSearchResponse
    {
        public List<FrankieSearchResults> PersonResults { get; set; } = new List<FrankieSearchResults>();
    }

    /// <summary>
    /// Represents a single matched person of interest
    /// </summary>
    public class FrankieSearchResults
    {
        public PersonDetail PersonDetails { get; set; } = new PersonDetail();
        public List<PEPDetail> PEPDetails { get; set; } = new List<PEPDetail>();
        public List<SanctionsDetail> SanctionsDetails { get; set; } = new List<SanctionsDetail>();
        public List<WatchlistDetail> WatchlistDetails { get; set; } = new List<WatchlistDetail>();
        public MediaDetail MediaDetails { get; set; }
    }

    public class MediaDetail
    {
        public MediaMentions Mentions { get; set; }

        public List<AdverseMediaDetail> Adverse { get; set; } = new List<AdverseMediaDetail>();
    }

    public class MediaMentions : GeneralDetail, IUniqueReportingDetail
    {
        public List<MediaRecordDetail> Mentions { get; set; } = new List<MediaRecordDetail>();

        public List<string> CheckTypes { get; set; }

        public string CaseId { get; set; }
        public string ReferenceId { get; set; }
        public string GroupingId { get; set; }
    }

    public class AdverseMediaDetail : GeneralDetail, IUniqueReportingDetail
    {
        public string CountryCodes { get; set; }
        public string Country { get; set; }
        public string Subtypes { get; set; }
        public List<string> CheckTypes { get; set; }
        public string CaseId { get; set; }
        public string ReferenceId { get; set; }
        public string GroupingId { get; set; }
    }

    public abstract class GeneralDetail
    {
        /// <summary>
        /// Provider's unique entity identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// SOURCE.name
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// original_checksource
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// SOURCE.url
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// SOURCE.aml_types
        /// </summary>
        public List<string> AmlTypes { get; set; }

        /// <summary>
        /// SOURCE.country_codes
        /// </summary>
        public string SourceCountryCodes { get; set; }

        /// <summary>
        /// SOURCE.country_names
        /// </summary>
        public string SourceCountryNames { get; set; }

        /// <summary>
        /// Original Country Text
        /// </summary>
        public List<string> OriginalCountries { get; set; }

        /// <summary>
        /// Countries || Country
        /// </summary>
        public List<string> Countries { get; set; }

        public List<Tuple<string, string>> AdditionalFields { get; set; } = new List<Tuple<string, string>>();
    }

    /// <summary>
    /// // Matching Criteria, scores, alias, associates, external document references, imageUrl
    /// </summary>
    public class PersonDetail : GeneralDetail
    {
        /// <summary>
        /// Matched full name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Matched DOB
        /// </summary>
        public string Dob { get; set; }
        /// <summary>
        /// Matched countries
        /// </summary>
        public List<string> Countries { get; set; }
        /// <summary>
        /// The url for the authority search report, e.g. ComplyAdvantage "https://app.complyadvantage.com/public/entity/1611549630-4jJ6aoSN/354c8e215280/N0HUXBOHUAA52RH"
        /// </summary>
        public Uri SearchUrl { get; set; }
        /// <summary>
        /// The url for the authority report, e.g. ComplyAdvantage "https://app.complyadvantage.com/public/entity/1611549630-4jJ6aoSN/354c8e215280/N0HUXBOHUAA52RH/{providerEntityCode}"
        /// </summary>
        public Uri ReportUrl { get; set; }
        /// <summary>
        /// Confidence score
        /// </summary>
        public decimal AmlScore { get; set; }
        public List<string> Alias { get; set; }
        public string ImageUrl { get; set; }
        public List<string> MatchTypes { get; set; }
        public Dictionary<string, List<string>> Associates { get; set; }

        public SearchDetails SearchDetails { get; set; } = new SearchDetails();
    }

    public class SearchDetails : IUniqueReportingDetail
    {
        public string Name { get; set; }
        public string Dob { get; set; }
        public string YearOfBirth { get; set; }
        public List<string> Countries { get; set; }

        public decimal Fuzziness { get; set; }
        public List<string> CheckTypes { get; set; }

        public string CaseId { get; set; }

        /// <summary>
        /// Provider's unique entity identifier (current provider is Frankie Financial)
        /// </summary>
        public string ProviderEntityCode { get; set; }
        public string ReferenceId { get; set; }
        public string GroupingId { get; set; }
    }

    public class PEPDetail : GeneralDetail//, IUniqueReportingDetail
    {
        public string Country { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Nationality { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public IEnumerable<string> ReferenceDocUrl { get; set; } = new List<string>();
        public List<string> Positions { get; set; }
        public List<string> CheckTypes { get; set; }

        public string ActiveStartDate { get; set; }
        public string ActiveEndDate { get; set; }
    }

    public class SanctionsDetail : GeneralDetail//, IUniqueReportingDetail
    {
        public string SourceListingStarted { get; set; }
        public string CountryCodes { get; set; }
        public string OriginalCountry { get; set; }
        public string DateOfBirthForSanction { get; set; }
        public string AdditionalInfo { get; set; }
        public string SanctionAddress { get; set; }

        public string DesignationDate { get; set; }
        public string LegalBasis { get; set; }

        public string SDNList { get; set; }

        public string ListingId { get; set; }
        public string OtherInformation { get; set; }
        public string Program { get; set; }
        public string SanctionType { get; set; }
        public string Title { get; set; }
        public List<string> CheckTypes { get; set; }
    }

    public class MediaRecordDetail
    {
        public string Date { get; set; }
        public string Pdf_url { get; set; }
        public string Snippet { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }

    public class WatchlistDetail : GeneralDetail//, IUniqueReportingDetail
    {
        public string DataSourceDate { get; set; }
    }
}

