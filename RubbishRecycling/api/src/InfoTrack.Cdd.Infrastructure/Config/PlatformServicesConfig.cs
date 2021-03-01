using System;

namespace InfoTrack.Cdd.Infrastructure.Config
{
    public class PlatformServicesConfig
    {
        public Uri PdfServiceUrl { get; set; }
        public Uri TemplateServiceUrl { get; set; }
        public Uri StorageServiceUrl { get; set; }
        public Uri EmailServiceUrl { get; set; }
    }
}
