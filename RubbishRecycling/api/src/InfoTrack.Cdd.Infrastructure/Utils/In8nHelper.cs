using System;
using System.Globalization;
using System.Linq;
using InfoTrack.Cdd.Application.Common.Enums;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace InfoTrack.Cdd.Infrastructure.Utils
{
    /// <summary>
    /// Internationalisation helper
    /// </summary>
    public static class In8nHelper
    {
        private static readonly string[] SupportedCultures = { "en-AU", "en-GB", "en-US" };
        private static readonly string[] SupportedRegions = { "AU", "UK", "US" };

        /// <summary>
        /// CultureInfo (en-AU, en-GB or en-US)
        /// </summary>
        public static CultureInfo CultureInfo { get; private set; } = new CultureInfo("en-AU");

        /// <summary>
        /// Region (AU, UK or US)
        /// </summary>
        public static string Region { get; private set; } = "AU";

        /// <summary>
        /// Iso2 Code (AU, GB or US)
        /// </summary>
        public static string Iso2 { get; } = Region == "UK" ? "GB" : Region;

        /// <summary>
        /// Document page size (A4 or Letter)
        /// </summary>
        public static DocumentPageSize? DocumentPageSize { get; private set; } = Application.Common.Enums.DocumentPageSize.A4; // default A4

        public static void Init(IConfiguration configuration)
        {
            if (configuration == null)
            {
                return;
            }

            var cultureInfo = configuration["Culture"];
            var region = configuration["Serilog:Properties:Region"];

            if (SupportedCultures.Contains(cultureInfo))
            {
                CultureInfo = new CultureInfo(cultureInfo);
            }

            if (SupportedRegions.Contains(region))
            {
                Region = region;
            }

            if (Enum.TryParse(configuration["DocumentPageSize"], out DocumentPageSize documentPageSize))
            {
                DocumentPageSize = documentPageSize;
            }
        }
    }
}
