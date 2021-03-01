using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoTrack.Cdd.Infrastructure.Constants;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared;
using InfoTrack.Cdd.Infrastructure.Utils;
using Newtonsoft.Json;
using static InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch.PersonSearchCriteria;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class CheckEntityCheckResultObjectExtensions
    {
        public static PersonSearchResponse TransformAuthorityResponse(this CheckEntityCheckResultObject self, EntityRequest request)
        {
            if (self?.EntityResults?.AmlResultsSet == null)
            {
                return null;
            }

            var personSearchResponse = new PersonSearchResponse();

            foreach (var amlResultSet in self.EntityResults.AmlResultsSet)
            {
                var frankieSearchResults = new FrankieSearchResults();

                #region Personal Details
                // Matching Criteria, scores, alias, associates, external document references, imageUrl
                var groupDetails = amlResultSet?.GroupDetails?.CheckDetails;
                if (groupDetails != null)
                {
                    frankieSearchResults.PersonDetails = new PersonDetail().WithValues(groupDetails).WithRequest(request); ;
                }
                #endregion

                #region PEPs

                if (amlResultSet?.CheckResultsListPEPs != null)
                {
                    // Each record will represent a single PEP listing. 
                    // For career politicians, there can often be many(10 +) of these.
                    foreach (var checkResultsListPEPs in amlResultSet.CheckResultsListPEPs)
                    {
                        var pepDetails = checkResultsListPEPs?.CheckDetails;
                        if (pepDetails != null)
                        {
                            frankieSearchResults.PEPDetails.Add(new PEPDetail().WithValues(pepDetails));
                        }
                    }
                }

                #endregion

                #region Sanctions
                if (amlResultSet?.CheckResultsListSanctions != null)
                {
                    foreach (var checkResultsListSanctions in amlResultSet?.CheckResultsListSanctions)
                    {
                        var sanctionDetails = checkResultsListSanctions?.CheckDetails;
                        if (sanctionDetails != null)
                        {
                            frankieSearchResults.SanctionsDetails.Add(new SanctionsDetail().WithValues(sanctionDetails));
                        }
                    }
                }
                #endregion

                #region Watchlists

                if (amlResultSet?.CheckResultsListWatchlists != null)
                {
                    // Each record will represent a single watchlist listing
                    foreach (var watchlists in amlResultSet.CheckResultsListWatchlists)
                    {
                        var watchlistDetails = watchlists?.CheckDetails;
                        if (watchlistDetails != null)
                        {
                            frankieSearchResults.WatchlistDetails.Add(new WatchlistDetail().WithValues(watchlistDetails));
                        }
                    }
                }

                #endregion

                #region AdverseMedia 
                if (amlResultSet?.CheckResultsListMedias != null)
                {
                    frankieSearchResults.MediaDetails = new MediaDetail();
                    foreach (var checkResultsListMedia in amlResultSet.CheckResultsListMedias)
                    {
                        var details = checkResultsListMedia.CheckDetails;
                        if (details != null)
                        {
                            if (details.ContainsKey("MediaDetails")) // Mentions are distinct from adverse media based on this key (weird but true)
                            {
                                var mediaDetails = details.GetKeyValue("MediaDetails");
                                if (mediaDetails != null)
                                {
                                    frankieSearchResults.MediaDetails.Mentions = new MediaMentions().WithValues(details);
                                }
                            }
                            else
                            {
                                frankieSearchResults.MediaDetails.Adverse.Add(new AdverseMediaDetail().WithValues(details));
                            }
                        }
                    }
                }

                #endregion
                personSearchResponse.PersonResults.Add(frankieSearchResults);
            }

            return personSearchResponse;
        }

        private static string[] GeneralDetailsFields =
        {
            "SOURCE.name",
            "original_checksource",
            "SOURCE.url",
            "SOURCE.aml_types",
            "SOURCE.country_codes",
            "Country",
            "Original Country Text",
            "Countries",
            "SOURCE.country_names",
            "aml.search_entity_id"
        };

        private static string[] StandardPersonFields =
        {
            "aml.search_name.matched",
            "aml.search_date_of_birth.matched",
            "aml.search_countries.matched",
            "aml.search_result.report_url",
            "aml.search_result.score",
            "aml.search_entity_aka",
            "aml.search_imageurl",
            "aml.search_name",
            "aml.search_date_of_birth",
            "aml.search_countries",
            "aml.search_match_types",
            "aml.search_fuzziness",
            "aml.search_reference_id",
            "aml.search_entity_id",
            "aml.case_id",
            "bcro.grouping_id",
            "requested_check_type"
        };

        private static string[] StandardPepFields =
        {
            "Original Country Text",
            "Original Place of Birth Text",
            "Nationality",
            "Date of Birth",
            "Gender",
            "reference.doc",
            "aml.search_entity_id",
            "requested_check_type",
            "Countries",
            "Political Position",
            "Active Start Date",
            "Active End Date",
            "aml.case_id",
            "aml.search_reference_id",
            "bcro.grouping_id"
        };

        private static string[] StandardSanctionFields =
        {
            "aml.case_id",
            "aml.search_reference_id",
            "aml.search_entity_id",
            "bcro.grouping_id",
            "SOURCE.listing_started",
            "Original Country Text",
            "Date of Birth",
            "Additional Sanctions Information",
            "Address",
            "Designation Date",
            "Legal Basis",
            "List Name",
            "Listing Id",
            "Other Information",
            "Program",
            "Sanction Type",
            "Title",
            "requested_check_type"
        };

        private static string[] StandardWatchlistFields =
        {
            "bcro.grouping_id",
            "requested_check_type",
            "aml.datasource.date"
        };

        private static string[] StandardMentionMediaFields =
        {
            "aml.search_reference_id",
            "aml.search_entity_id",
            "aml.case_id",
            "bcro.grouping_id",
            "requested_check_type"
        };

        private static string[] StandardAdverseMediaFields =
        {
            "aml.search_reference_id",
            "aml.search_entity_id",
            "aml.case_id",
            "bcro.grouping_id",
            "Original Country Text",
            "Adverse Media Subtypes",
            "requested_check_type"
        };

        private static GeneralDetail InfillGeneralDetails(this GeneralDetail detail, List<KeyValuePairs> values)
        {
            if (detail != null)
            {
                detail.ProviderEntityCode = values.GetKeyValue("aml.search_entity_id");
                detail.SourceName = values.GetKeyValue("SOURCE.name");
                detail.Source = values.GetKeyValue("original_checksource");
                detail.SourceUrl = values.GetKeyValue("SOURCE.url");
                detail.AmlTypes = values.GetKeyValueList("SOURCE.aml_types").ToList();
                detail.SourceCountryCodes = values.GetKeyValue("SOURCE.country_codes");
                detail.SourceCountryNames = values.GetKeyValue("SOURCE.country_names");
                detail.OriginalCountries = values.GetKeyValueList("Original Country Text").ToList();

                detail.Countries = values.GetKeyValue("Countries")?.Split(",")?.ToList();
                var moreCountries = values.GetKeyValue("Country")?.Split(",")?.ToList();
                if (moreCountries != null)
                {
                    foreach (var c in moreCountries.Where(cc => !string.IsNullOrWhiteSpace(cc)))
                    {
                        detail.Countries.Add(c);
                    }
                }
                detail.Countries = detail.Countries.Distinct()
                    .Where(c => !string.IsNullOrWhiteSpace(c) && c.Trim() != ",").ToList();
            }

            return detail;
        }

        private static PersonDetail WithValues(this PersonDetail detail, List<KeyValuePairs> values)
        {
            if (detail != null)
            {
                detail.InfillGeneralDetails(values);

                detail.FullName = values.GetKeyValue("aml.search_name.matched");
                detail.Dob = values.GetKeyValue("aml.search_date_of_birth.matched");
                detail.Countries = values.GetKeyValue("aml.search_countries.matched")?.Split(",")?.ToList();

                detail.AmlScore = Decimal.TryParse(values.GetKeyValue("aml.search_result.score"), out decimal dec1) ? dec1 : 0;
                detail.Alias = values.GetKeyValue("aml.search_entity_aka")?.Split(",")?.ToList();
                detail.ImageUrl = values.GetKeyValue("aml.search_imageurl");
                detail.MatchTypes = values.GetKeyValue("aml.search_match_types")?.Split(",")?.ToList();
                detail.Associates = new Dictionary<string, List<string>>(values.GetKeyValueDictonary("aml.search_entity_associate"));

                detail.SearchDetails.Name = values.GetKeyValue("aml.search_name");
                detail.SearchDetails.Dob = values.GetKeyValue("aml.search_date_of_birth");
                detail.SearchDetails.Countries = values.GetKeyValue("aml.search_countries")?.Split(",")?.ToList();
                detail.SearchDetails.Fuzziness = Decimal.TryParse(values.GetKeyValue("aml.search_fuzziness"), out decimal dec2) ? dec2 : 0;
                detail.SearchDetails.ReferenceId = values.GetKeyValue("aml.search_reference_id");
                detail.SearchDetails.CaseId = values.GetKeyValue("aml.case_id");
                detail.SearchDetails.GroupingId = values.GetKeyValue("bcro.grouping_id");
                detail.SearchDetails.CheckTypes = values.GetKeyValue("requested_check_type")?.Split(",")?.ToList();

                if (detail.SearchDetails.Countries == null || detail.SearchDetails.Countries.All(c => string.IsNullOrWhiteSpace(c)))
                {
                    detail.SearchDetails.Countries = new List<string> { "ALL" };
                }

                foreach (var kvp in values.Where(k =>
                    !StandardPersonFields.Concat(GeneralDetailsFields).Contains(k.KvpKey) &&
                    !k.KvpKey.StartsWith("aml.search_entity_associate.")))
                {
                    var key = ReportHelper.ReplaceHeadings(ReportHelper.PersonDetailKeywords, kvp.KvpKey);
                    detail.AdditionalFields.Add(new Tuple<string, string>(key, kvp.KvpValue));
                }

                if (string.IsNullOrWhiteSpace(detail.ProviderEntityCode))
                {
                    detail.ProviderEntityCode = Defaults.NilResult;
                }

                var searchReport = values.GetKeyValue("aml.search_result.report_url");
                if (!string.IsNullOrWhiteSpace(searchReport))
                {
                    detail.SearchUrl = new Uri(searchReport);
                    if (!searchReport.EndsWith(detail.ProviderEntityCode))
                    {
                        // Perhaps this logic will not always be true, but we handle these report Urls gracefully anyway in event of failure
                        detail.ReportUrl = new Uri($"{searchReport.Replace("public/search", "public/entity")}/{detail.ProviderEntityCode}");
                    }
                }
            }

            return detail;
        }

        private static PersonDetail WithRequest(this PersonDetail detail, EntityRequest request)
        {
            if (detail != null)
            {
                if (request?.Entity != null)
                {
                    if (string.IsNullOrWhiteSpace(detail.SearchDetails.Name))
                    {
                        detail.SearchDetails.Name = request.Entity.Name?.ToString();
                    }

                    if (string.IsNullOrWhiteSpace(detail.SearchDetails.Dob))
                    {
                        detail.SearchDetails.Dob = request.Entity.DateOfBirth?.ToString();
                    }
                }
            }

            return detail;
        }

        private static PEPDetail WithValues(this PEPDetail detail, List<KeyValuePairs> values)
        {
            if (detail != null)
            {
                detail.InfillGeneralDetails(values);

                detail.Positions = values.GetKeyValueList("Political Position").ToList();
                detail.DateOfBirth = values.GetKeyValue("Date of Birth");
                detail.PlaceOfBirth = values.GetKeyValue("Original Place of Birth Text");
                detail.Nationality = values.GetKeyValue("Nationality");
                detail.Gender = values.GetKeyValue("Gender");
                detail.ReferenceDocUrl = values.GetKeyValueList("reference.doc");
                detail.CheckTypes = values.GetKeyValueList("requested_check_type").ToList();
                detail.ActiveStartDate = values.GetKeyValue("Active Start Date");
                detail.ActiveEndDate = values.GetKeyValue("Active End Date");

                foreach (var kvp in values.Where(k => !StandardPepFields.Concat(GeneralDetailsFields).Contains(k.KvpKey)))
                {
                    var key = ReportHelper.ReplaceHeadings(ReportHelper.PoliticalKeywords, kvp.KvpKey);
                    detail.AdditionalFields.Add(new Tuple<string, string>(key, kvp.KvpValue));
                }
            }

            return detail;
        }

        private static WatchlistDetail WithValues(this WatchlistDetail detail, List<KeyValuePairs> values)
        {
            if (detail != null)
            {
                detail.InfillGeneralDetails(values);

                detail.DataSourceDate = values.GetKeyValue("aml.datasource.date");

                foreach (var kvp in values.Where(k => !StandardWatchlistFields.Concat(GeneralDetailsFields).Contains(k.KvpKey)))
                {
                    var key = ReportHelper.ReplaceHeadings(ReportHelper.WatchListKeywords, kvp.KvpKey);
                    detail.AdditionalFields.Add(new Tuple<string, string>(key, kvp.KvpValue));
                }
            }

            return detail;
        }

        private static SanctionsDetail WithValues(this SanctionsDetail detail, List<KeyValuePairs> values)
        {
            if (detail != null)
            {
                detail.InfillGeneralDetails(values);

                detail.DateOfBirthForSanction = values.GetKeyValue("Date of Birth");
                detail.AdditionalInfo = values.GetKeyValue("Additional Sanctions Information");
                detail.SanctionAddress = values.GetKeyValue("Address");
                detail.DesignationDate = values.GetKeyValue("Designation Date");
                detail.LegalBasis = values.GetKeyValue("Legal Basis");
                detail.SDNList = values.GetKeyValue("List Name");
                detail.ListingId = values.GetKeyValue("Listing Id");
                detail.OtherInformation = values.GetKeyValue("Other Information");
                detail.Program = values.GetKeyValue("Program");
                detail.SanctionType = values.GetKeyValue("Sanction Type");
                detail.Title = values.GetKeyValue("Title");
                detail.SourceListingStarted = values.GetKeyValue("SOURCE.listing_started");
                detail.AmlTypes = values.GetKeyValueList("SOURCE.aml_types").ToList();
                detail.CheckTypes = values.GetKeyValueList("requested_check_type").ToList();

                foreach (var kvp in values.Where(k => !StandardSanctionFields.Concat(GeneralDetailsFields).Contains(k.KvpKey)))
                {
                    var key = ReportHelper.ReplaceHeadings(ReportHelper.SanctionsKeywords, kvp.KvpKey);
                    detail.AdditionalFields.Add(new Tuple<string, string>(key, kvp.KvpValue));
                }
            }

            return detail;
        }

        private static MediaMentions WithValues(this MediaMentions detail, List<KeyValuePairs> values)
        {
            if (detail != null)
            {
                detail.InfillGeneralDetails(values);

                detail.ReferenceId = values.GetKeyValue("aml.search_reference_id");
                detail.CaseId = values.GetKeyValue("aml.case_id");
                detail.GroupingId = values.GetKeyValue("bcro.grouping_id");
                detail.CheckTypes = values.GetKeyValue("requested_check_type")?.Split(",")?.ToList();

                foreach (var kvp in values.Where(k => !StandardMentionMediaFields.Concat(GeneralDetailsFields).Contains(k.KvpKey)))
                {
                    detail.AdditionalFields.Add(new Tuple<string, string>(kvp.KvpKey, kvp.KvpValue));
                }

                var mediaDetails = values.GetKeyValue("MediaDetails");
                if (mediaDetails != null)
                {
                    string decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(mediaDetails));
                    if (!string.IsNullOrEmpty(decodedString))
                    {
                        detail.Mentions.AddRange(JsonConvert.DeserializeObject<List<MediaRecordDetail>>(decodedString));
                    }
                }
            }

            return detail;
        }

        private static AdverseMediaDetail WithValues(this AdverseMediaDetail detail, List<KeyValuePairs> values)
        {
            if (detail != null)
            {
                detail.InfillGeneralDetails(values);

                detail.ReferenceId = values.GetKeyValue("aml.search_reference_id");
                detail.CaseId = values.GetKeyValue("aml.case_id");
                detail.GroupingId = values.GetKeyValue("bcro.grouping_id");

                detail.AmlTypes = values.GetKeyValueList("SOURCE.aml_types").ToList();
                detail.Subtypes = values.GetKeyValue("Adverse Media Subtypes");
                detail.CheckTypes = values.GetKeyValue("requested_check_type")?.Split(",")?.ToList();

                foreach (var kvp in values.Where(k => !StandardAdverseMediaFields.Concat(GeneralDetailsFields).Contains(k.KvpKey)))
                {
                    var key = ReportHelper.ReplaceHeadings(ReportHelper.AdverseMediaKeywords, kvp.KvpKey);
                    detail.AdditionalFields.Add(new Tuple<string, string>(key, kvp.KvpValue));
                }
            }

            return detail;
        }

        private static string GetKeyValue<T>(this T Details, string searchString) where T : List<KeyValuePairs>
        {
            // Even if only expecting one item, handle as if there may be more because we shouldn't risk overwriting
            return string.Join(", ", Details.GetKeyValueList(searchString));
        }

        private static bool ContainsKey<T>(this T Details, string searchString) where T : List<KeyValuePairs>
        {
            return Details.Any(x =>
                    !string.IsNullOrWhiteSpace(x.KvpKey) &&
                    x.KvpKey == searchString);
        }

        private static IEnumerable<string> GetKeyValueList<T>(this T Details, string searchString) where T : List<KeyValuePairs>
        {
            return Details.Where(x =>
                    !string.IsNullOrWhiteSpace(x.KvpKey) &&
                    x.KvpKey == searchString &&
                    x.KvpValue != null
                ).Select(k => k.KvpValue);
        }

        private static Dictionary<string, List<string>> GetKeyValueDictonary<T>(this T Details, string searchString) where T : List<KeyValuePairs>
        {
            var dict = new Dictionary<string, List<string>>();
            var results = Details.FindAll(x => !string.IsNullOrWhiteSpace(x.KvpKey) && x.KvpKey.Contains(searchString));

            foreach (var keyValue in results)
            {
                //Synthesize key names "aml.search_entity_associate.relative" -> relative
                string extractedKey = keyValue.KvpKey;
                if (extractedKey != null)
                {
                    int idx = extractedKey.LastIndexOf('.');
                    if (idx >= 0)
                    {
                        extractedKey = extractedKey.Substring(idx + 1);

                        if (dict.ContainsKey(keyValue.KvpKey))
                        {
                            dict[extractedKey].Add(keyValue.KvpValue);
                        }
                        else
                        {
                            dict[extractedKey] = new List<string> { keyValue.KvpValue };
                        }
                    }
                }
            }
            return dict;
        }
    }
}
