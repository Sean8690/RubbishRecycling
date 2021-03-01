using System.Collections.Generic;
using System.Linq;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class ActivityDeclarationDtoListExtensions
    {
        public static bool HasAnyDeclaration(this List<ActivityDeclarationDto> activities) 
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.Declaration));

        public static bool HasAnyDescription(this List<ActivityDeclarationDto> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.DeclarationDescription));

        public static bool HasAnyLanguage(this List<ActivityDeclarationDto> activities)
            => activities != null && activities.Any(a => !string.IsNullOrWhiteSpace(a?.Language));

        public static bool HasAnyData(this List<ActivityDeclarationDto> activities)
            => activities != null && (activities.HasAnyDeclaration() || activities.HasAnyDescription() || activities.HasAnyLanguage());
    }
}
