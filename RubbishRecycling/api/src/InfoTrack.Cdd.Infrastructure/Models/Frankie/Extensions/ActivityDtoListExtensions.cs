using System.Collections.Generic;
using System.Linq;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class ActivityDtoListExtensions
    {
        public static bool HasAnyCode(this List<ActivityDto> activities) 
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.Code) && a.Code != "-");

        public static bool HasAnyDescription(this List<ActivityDto> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.Description) && a.Description != "-");

        public static bool HasAnyData(this List<ActivityDto> activities)
            => activities != null && (activities.HasAnyCode() || activities.HasAnyDescription());
    }
}
