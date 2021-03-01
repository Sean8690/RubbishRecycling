using System.Collections.Generic;
using System.Linq;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class DirectorListExtensions
    {
        public static bool HasAnyTitle(this List<Director> directors, string includeCompanyName, string includeCompanyNumber)
            => directors != null && directors.Any(d => !string.IsNullOrWhiteSpace(d.GetTitle(includeCompanyName, includeCompanyNumber)));

        public static bool HasAnyAppointedDate(this List<Director> directors, string includeCompanyName, string includeCompanyNumber)
            => directors != null && directors.Any(d => !string.IsNullOrWhiteSpace(d.GetAppointedDate(includeCompanyName, includeCompanyNumber)));

        public static bool HasAnyBirthdate(this List<Director> directors)
            => directors != null && directors.Any(d => !string.IsNullOrWhiteSpace(d?.Birthdate));

        public static bool HasAnyNationality(this List<Director> directors)
            => directors != null && directors.Any(d => !string.IsNullOrWhiteSpace(d?.Nationality));

        public static bool HasAnyDirectorNumber(this List<Director> directors)
            => directors != null && directors.Any(d => !string.IsNullOrWhiteSpace(d?.DirectorNumber));
    }
}
