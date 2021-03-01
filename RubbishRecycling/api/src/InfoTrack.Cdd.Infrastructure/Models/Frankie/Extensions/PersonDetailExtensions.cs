using System.Collections.Generic;
using System.Linq;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class PersonDetailExtensions
    {
        private static Dictionary<string, string> CheckTypes = new Dictionary<string, string>()
        {
            { "one_plus", "Name, address and DoB checked against a minimum of 1 data source" },
            { "two_plus", "Name, address and DoB checked against a minimum of 2 independent data sources" },
            { "id", "Identity documents checked" },
            { "pep", "PEP/Sanctions checks" },
            { "pep_media", "PEP/Sanctions, watchlist and adverse media checks" },
            { "idvalidate", "Checks to see if photo ID has had OCR scanning, ID document validation and photo comparison run against it"}
        };

        public static string GetName(this PersonDetail detail)
        {
            return !string.IsNullOrWhiteSpace(detail?.FullName) ? detail.FullName : detail?.SearchDetails?.Name;
        }

        public static string GetDob(this PersonDetail detail)
        {
            return !string.IsNullOrWhiteSpace(detail?.Dob) ? GetDateOfBirth(detail.Dob) : !string.IsNullOrWhiteSpace(detail?.SearchDetails?.Dob) ?
                detail?.SearchDetails?.Dob : detail?.SearchDetails?.YearOfBirth;
        }

        public static List<string> GetCheckTypes(this PersonDetail detail)
        {
            return detail?.SearchDetails?.CheckTypes?
                .Where(c => !string.IsNullOrEmpty(c))
                .Select(c => CheckTypes.ContainsKey(c) ? CheckTypes[c] : c).ToList();
        }

        public static string GetSource(this GeneralDetail detail)
        {
            return !string.IsNullOrWhiteSpace(detail?.SourceName)
                ? detail.SourceName : detail?.Source;
        }

        public static List<string> GetSourceCountries(this GeneralDetail detail)
        {
            return (!string.IsNullOrWhiteSpace(detail?.SourceCountryNames)
                ? detail.SourceCountryNames : detail?.SourceCountryCodes)?
                    .Split(",")?
                    .Where(c => !string.IsNullOrWhiteSpace(c))?
                    .ToList();
        }

        public static bool HasMatch(this PersonDetail detail)
        {
            return !string.IsNullOrWhiteSpace(detail?.FullName) ||
                !string.IsNullOrWhiteSpace(detail?.Dob);
        }

        public static bool HasAssociates(this PersonDetail detail)
        {
            return detail?.Associates != null && detail.Associates.Any(a => a.Key != null && a.Value != null);
        }

        public static bool HasPep(this List<PEPDetail> details)
        {
            return details != null && details.Any();
        }

        public static bool HasMedia(this MediaDetail details)
        {
            return details != null && (details.HasAdverseMedia() || details.HasMediaMentions());
        }

        public static bool HasAdverseMedia(this MediaDetail details)
        {
            return details?.Adverse != null && details.Adverse.Any();
        }

        public static bool HasMediaMentions(this MediaDetail details)
        {
            return details?.Mentions?.Mentions != null && details.Mentions.Mentions.Any();
        }

        public static bool HasSanctions(this List<SanctionsDetail> details)
        {
            return details != null && details.Any();
        }

        public static bool HasWatchlists(this List<WatchlistDetail> details)
        {
            return details != null && details.Any();
        }

        private static string GetDateOfBirth(string dob)
        {
            return dob.Split(",")?.Where(dob => dob != null).OrderByDescending(dob => dob.Length).FirstOrDefault();
        }
    }
}
