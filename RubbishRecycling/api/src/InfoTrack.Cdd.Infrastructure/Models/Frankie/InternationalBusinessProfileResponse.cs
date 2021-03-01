using System;
using System.Collections.Generic;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared;
using InfoTrack.Cdd.Infrastructure.Utils;
using Newtonsoft.Json;

#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
// ReSharper disable UnusedMember.Global

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie
{
    /// <summary>
    /// Frankie: This wraps the search response details from Kyckr
    /// </summary>
    [JsonConverter(typeof(WrappedObjectListConverter))]
    public partial class InternationalBusinessProfileResponse : FrankieResponseBase
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

        /// <summary>
        /// Frankie: string($uuid)
        /// example: 84a9a860-68ae-4d7d-9a53-54a1116d5051
        /// If the response was successful and we returned a company profile, we save this as an ORGANISATION type entity in our service.
        /// We will also save the profile result as a REPORT type document, attached to the entity.
        /// </summary>
        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        /// <summary>
        /// Frankie: Unique ID for the individual check that was run. e.g. "26f6680e-9a31-2918-c9fa-36605ebeb903"
        /// </summary>
        [JsonProperty("checkId")]
        public string CheckId { get; set; }

        /// <summary>
        /// The url for the Kyckr report, e.g. "https://data.kyckr.com/integration/1260664578"
        /// </summary>
        [JsonProperty("ibRetrievalLocation")]
        public Uri KyckrReport { get; set; }

        public CompanyProfileDto CompanyProfile { get; set; }
    }

    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class CompanyProfileDto
    {
        [JsonProperty("Activity"), JsonWrappedObjectList("ActivityDTO")]
        public List<ActivityDto> Activities { get; set; }

        [JsonProperty("ActivityDeclaration"), JsonWrappedObjectList("ActivityDeclarationDTO")]
        public List<ActivityDeclarationDto> ActivityDeclarations { get; set; }

        [JsonWrappedObjectList("Addresses")]
        public List<AddressDto> Addresses { get; set; }

        public string AgentAddress { get; set; }
        
        public string AgentName { get; set; }
        
        public Aliases Aliases { get; set; }

        public string AppointmentDateOfOfficial { get; set; }

        [JsonWrappedObjectList("CapitalDTO")]
        public List<CapitalDto> Capital { get; set; }

        /// <summary>
        /// Frankie's unique code (the format varies significantly between countries)
        /// </summary>
        public string ProviderEntityCode { get; set; }

        /// <summary>
        /// e.g. "ACN: 551422854"
        /// </summary>
        [JsonProperty("Code")] 
        public string CompanyNumber { get; set; }

        public string CompanyNameInEnglish { get; set; }

        /// <summary>
        /// Looks like (but might not always be guaranteed to be) the data retrieval date e.g. "12/04/2020 12:40 AM" (from US-CA, US format assumed)
        /// </summary>
        public string Date { get; set; }

        public string Email { get; set; }

        public string FaxNumber { get; set; }

        public string FiscalCode { get; set; }

        /// <summary>
        /// AKA Incorporation Date e.g. "12/24/1963" (from US-CA, US format) or "2018-10-12" (from GB)
        /// </summary>
        public string FoundationDate { get; set; }

        public List<string> Functions { get; set; }

        public string Headquarters { get; set; }

        public List<string> KeyFigures { get; set; }

        public string LastAnnualAccountDate { get; set; }

        /// <summary>
        /// e.g. "Australian Proprietary Company Limited By Shares" or "Corporation - DOMESTIC STOCK" or "ldt"
        /// </summary>
        public string LegalForm { get; set; }

        public string LegalFormDeclaration { get; set; }

        public LegalFormDto LegalFormDetails { get; set; }

        /// <summary>
        /// e.g. "Registered" or "FTB SUSPENDED" or "active"
        /// </summary>
        public string LegalStatus { get; set; }

        public string MailingAddress { get; set; }

        public string Name { get; set; }

        public bool? Official { get; set; }

        /// <summary>
        /// e.g. "Secretary of State of California" or "This extract contains information derived from the Australian Securities and Investment Commission's (ASIC) database under section 1274A of the Corporations Act 2001. Please advise ASIC of any error which you may identify."
        /// </summary>
        public string RegistrationAuthority { get; set; }

        public string RegistrationAuthorityCode { get; set; }

        /// <summary>
        /// e.g. "23/04/2009"
        /// </summary>
        public string RegistrationDate { get; set; }

        public string RegistrationNumber { get; set; }

        public string SigningDeclaration { get; set; }

        public string SigningDeclarationDescription { get; set; }

        public string SigningLanguage { get; set; }

        public string Source { get; set; }

        /// <summary>
        /// e.g. "CALIFORNIA"
        /// </summary>
        public string StateOfIncorporation { get; set; }

        public string TelephoneNumber { get; set; }

        public string VatNumber { get; set; }

        public string VirtualId { get; set; }

        [JsonProperty("WebsiteURL")]
        #pragma warning disable CA1056 // Uri properties should not be strings
        public string WebsiteUrl { get; set; }
        #pragma warning restore CA1056 // Uri properties should not be strings

        [JsonProperty("directorAndShareDetails")]
        public DirectorAndShareDetails DirectorAndShareDetails { get; set; }

        [JsonWrappedObjectList("UsOfficerDto")]
        public List<UsOfficerDto> Officers { get; set; }
    }

    public class ActivityDto
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class ActivityDeclarationDto
    {
        public string Declaration { get; set; }

        public string DeclarationDescription { get; set; }

        public string Language { get; set; }
    }

    public class CapitalDto 
    {
        [JsonProperty("Ammount")] // typo from authority
        public string Amount { get; set; }

        public string Currency { get; set; }

        public string Type { get; set; }

        public string TypeCode { get; set; }

    }

    public class LegalFormDto
    {
        public string Basis { get; set; }

        public string Capital { get; set; }

        public string Comments { get; set; }

        public string Control { get; set; }

        public string Incorp { get; set; }

        public string Partner { get; set; }

        public string Responsibility { get; set; }

        public string Stocks { get; set; }
    }

    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class DirectorAndShareDetails
    {
        [JsonProperty("directors"), JsonWrappedObjectList("Director")]
        public List<Director> Directors { get; set; }

        [JsonWrappedObjectList("PscDetails")]
        public List<PscDetails> PersonsOfSignificantControl { get; set; }

        public CapitalReserves CapitalReserves { get; set; }

        [JsonProperty("shareHolderSummary")]
        public ShareholderSummary ShareholderSummary { get; set; }

        [JsonProperty("shareHolders"), JsonWrappedObjectList("ShareholderDetails")]
        public List<ShareholderDetails> Shareholders { get; set; }
    }

    public class PscDetails 
    {
        public string Name { get; set; }

        public string Kind { get; set; }

        public string Nationality { get; set; }
        
        public string CountryOfResidence { get; set; }

        public string Address { get; set; }

        public string CeasedOn { get; set; }

        [JsonProperty("DOBDay")]
        public Int64? DobDay { get; set; }

        [JsonProperty("DOBMonth")]
        public Int64? DobMonth { get; set; }

        [JsonProperty("DOBYear")]
        public Int64? DobYear { get; set; }

        public List<string> NatureOfControl { get; set; } // TODO there is something weird about the Frankie definiton for this prop 

        public string NotifiedOn { get; set; }
    }

    public class CapitalReserves
    {
        [JsonProperty("capitalreserves")]
        public string CapReserves { get; set; }

        [JsonProperty("networth")]
        public string NetWorth { get; set; }

        [JsonProperty("paidupequity")]
        public string PaidUpEquity { get; set; }

        [JsonProperty("profitlossreserve")]
        public string ProfitLossReserve { get; set; }

        [JsonProperty("reserves")]
        public string Reserves { get; set; }

        [JsonProperty("revalutationreserve")] // typo
        public string RevaluationReserve { get; set; }

        [JsonProperty("shareholderfunds")]
        public string ShareholderFunds { get; set; }

        [JsonProperty("sundryreserves")]
        public string SundryReserves { get; set; }
    }

    [JsonConverter(typeof(WrappedObjectListConverter))]
    public class Director
    {
        /// <summary>
        /// e.g. "Director" or "Secretary"
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


        /// <summary>
        /// e.g. "01/01/1970 at TRARALGON, VIC"
        /// </summary>
        [JsonProperty("birthdate")]
        public string Birthdate { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("directorNumber")]
        public string DirectorNumber { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("address3")]
        public string Address3 { get; set; }

        [JsonProperty("address4")]
        public string Address4 { get; set; }

        [JsonProperty("address5")]
        public string Address5 { get; set; }

        [JsonProperty("address6")]
        public string Address6 { get; set; }

        [JsonProperty("directorships"), JsonWrappedObjectList("Directorship")]
        public List<Directorship> Directorships { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }
    }

    public class Directorship
    {
        [JsonProperty("appointedDate")]
        public string AppointedDate { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("companyNumber")]
        public string CompanyNumber { get; set; }

        [JsonProperty("companyStatus")]
        public string CompanyStatus { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }
    }

    public class ShareholderSummary 
    {
        public string ShareCapital { get; set; }
    }

    public class ShareholderDetails
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string ShareholderType { get; set; }

        public string Nationality { get; set; }

        /// <summary>
        /// e.g. "UNIT 1, 12 REGENCY COURT, TRARALGON, VIC, 3844"
        /// </summary>
        public string Address { get; set; }

        public string AllInfo { get; set; }

        public string Currency { get; set; }

        public string NominalValue { get; set; }

        public string Percentage { get; set; }

        /// <summary>
        /// e.g. "A" or "ORD"
        /// </summary>
        public string ShareClass { get; set; }

        public Int64? ShareCount { get; set; }

        /// <summary>
        /// e.g. "Current"
        /// </summary>
        public string ShareType { get; set; }

        public Int64? TotalShareCount { get; set; }

        public Int64? TotalShareValue { get; set; }

        public Int64? TotalShares { get; set; }
    }

    public class UsOfficerDto
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Date { get; set; }

        public string Address { get; set; }

        // API contract is confusing here
        //public List<String1> BusinessAddress { get; set; }
        public List<string> BusinessAddress { get; set; }

        public string MailingAddress { get; set; }
   
    }
}
