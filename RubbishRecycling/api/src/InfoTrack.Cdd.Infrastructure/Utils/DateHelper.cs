using System;

namespace InfoTrack.Cdd.Infrastructure.Utils
{
    public static class DateHelper
    {
        public static string TryFormatShortDate(string date)
        {
            if (DateTime.TryParse(date, out DateTime dt) && dt != DateTime.MaxValue && dt != DateTime.MinValue)
            {
                return dt.ToString(In8nHelper.CultureInfo.DateTimeFormat.ShortDatePattern, In8nHelper.CultureInfo);
            }

            return date;
        }

        public static string FormatShortDate(DateTime dt)
            => dt == DateTime.MaxValue || dt == DateTime.MinValue ? null : dt.ToString(In8nHelper.CultureInfo.DateTimeFormat.ShortDatePattern, In8nHelper.CultureInfo);

        public static string FormatShortTime(DateTime dt)
            => dt == DateTime.MaxValue || dt == DateTime.MinValue ? null : dt.ToString(In8nHelper.CultureInfo.DateTimeFormat.ShortTimePattern, In8nHelper.CultureInfo);

        public static string FormatLongDate(DateTime dt)
            => dt == DateTime.MaxValue || dt == DateTime.MinValue ? null : dt.ToString(In8nHelper.CultureInfo.DateTimeFormat.LongDatePattern, In8nHelper.CultureInfo);
        
        public static string FormatLongTime(DateTime dt)
            => dt == DateTime.MaxValue || dt == DateTime.MinValue ? null : dt.ToString(In8nHelper.CultureInfo.DateTimeFormat.LongTimePattern, In8nHelper.CultureInfo);
    }
}
