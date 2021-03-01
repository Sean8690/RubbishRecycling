namespace InfoTrack.Cdd.Application.Common.Constants
{
    /// <summary>
    /// Regex patterns
    /// </summary>
    public static class Regexes
    {
        /// <summary>
        /// KyckrCountryCode must match this regex pattern
        /// </summary>
        public const string KyckrCountryCode = "^[A-Z][A-Z](-[A-Z][A-Z])?$";

        /// <summary>
        /// Match year (e.g. 2021)
        /// </summary>
        public const string Year = "^\\d{4}$";

        /// <summary>
        /// Match date (e.g. 2021-12-05)
        /// </summary>
        public const string Date = "^\\d{4}-\\d{2}-\\d{2}$";
    }
}
