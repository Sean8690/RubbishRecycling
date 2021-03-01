using InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class MediaDetailExtensions
    {
        public static string GetUrl(this MediaRecordDetail mediaDetail)
        {
            if (!string.IsNullOrWhiteSpace(mediaDetail?.Url))
            {
                return mediaDetail?.Url;
            }
            if (!string.IsNullOrWhiteSpace(mediaDetail?.Pdf_url))
            {
                return mediaDetail?.Pdf_url;
            }
            return null;
        }
    }
}
