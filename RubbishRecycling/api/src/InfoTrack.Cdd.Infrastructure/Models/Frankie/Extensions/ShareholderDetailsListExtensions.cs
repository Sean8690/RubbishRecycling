using System.Collections.Generic;
using System.Linq;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class ShareholderDetailsListExtensions
    {
        public static bool HasAnyId(this List<ShareholderDetails> activities) 
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.Id));

        public static bool HasAnyType(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.ShareholderType));

        public static bool HasAnyNationality(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.Nationality));

        public static bool HasAnyAddress(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.Address));
        
        public static bool HasAnyAllInfo(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.AllInfo));

        public static bool HasAnyCurrency(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.Currency));

        public static bool HasAnyNominalValue(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.NominalValue));

        public static bool HasAnyPercentage(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.Percentage));

        public static bool HasAnyShareClass(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.ShareClass));

        public static bool HasAnyShareCount(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => a?.ShareCount != null);

        public static bool HasAnyShareType(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.ShareType));

        public static bool HasAnyTotalShareCount(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => a?.TotalShareCount != null);

        public static bool HasAnyTotalShareValue(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => a?.TotalShareValue != null);

        public static bool HasAnyTotalShares(this List<ShareholderDetails> activities)
            => activities != null && activities.Any(a => a?.TotalShares != null);
    }
}
