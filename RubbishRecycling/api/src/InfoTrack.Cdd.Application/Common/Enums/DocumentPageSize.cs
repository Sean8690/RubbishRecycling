using System.Linq;
using InfoTrack.Cdd.Application.Common.Attributes;

namespace InfoTrack.Cdd.Application.Common.Enums
{
    /// <summary>
    /// Document page size
    /// </summary>
    public enum DocumentPageSize
    {
        /// <summary>
        /// Default: A4 (210 x 297 mm)
        /// </summary>
        [Dimensions(210, 297)]
        Default,
        /// <summary>
        /// 210 x 297 mm
        /// </summary>
        [Dimensions(210, 297)]
        A4,
        /// <summary>
        /// 215.9 x 279.4 mm
        /// </summary>
        [Dimensions(215.9, 279.4)]
        Letter
    }

    /// <summary>
    /// Extensions for DocumentPageSize Enum
    /// </summary>
    public static class DocumentPageSizeExtensions
    {
        /// <summary>
        /// Get (portrait mode) page width
        /// </summary>
        public static double? Width(this DocumentPageSize? documentPageSize) 
            => documentPageSize != null ? GetDimensions(documentPageSize.Value)?.Width : null;

        /// <summary>
        /// Get (portrait mode) page height
        /// </summary>
        public static double? Height(this DocumentPageSize? documentPageSize)
            => documentPageSize != null ? GetDimensions(documentPageSize.Value)?.Height : null;

        private static DimensionsAttribute GetDimensions(DocumentPageSize documentPageSize)
        {
            var memberInfo = typeof(DocumentPageSize)
                .GetMember(documentPageSize.ToString())
                .Single();

            if (memberInfo != null)
            {
                var attribute = (DimensionsAttribute)
                    memberInfo.GetCustomAttributes(typeof(DimensionsAttribute), false)
                        .SingleOrDefault();
                return attribute;
            }

            return null;
        }
    }
}
