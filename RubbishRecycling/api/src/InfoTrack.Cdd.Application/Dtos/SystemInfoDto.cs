namespace InfoTrack.Cdd.Application.Dtos
{
    /// <summary>
    /// System and region info
    /// </summary>
    public class SystemInfoDto
    {
        /// <summary>
        /// Assembly name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Assembly version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Application name (from Serilog settings)
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Environment (from Serilog settings)
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Region (from Serilog settings)
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Timezone ID
        /// </summary>
        public string TimeZoneId { get; set; }
    }

    /// <summary>
    /// Private system and region info
    /// </summary>
    public class PrivateSystemInfoDto
    {
        /// <summary>
        /// Authentication authority (1)
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Authentication authority (2) // TODO work out why there are 2, and if one can be removed
        /// </summary>
        public string Authentication { get; set; }

        /// <summary>
        /// Frankie API Uri
        /// </summary>
        public string FrankieUri { get; set; }

        /// <summary>
        /// Frankie CustomerID
        /// </summary>
        public string FrankieCustomerId { get; set; }

        /// <summary>
        /// Mongo connection string
        /// </summary>
        public string Mongo { get; set; }

        /// <summary>
        /// Platform Pdf Service Url
        /// </summary>
        public string PdfService { get; set; }

        /// <summary>
        /// Platform Template Service Url
        /// </summary>
        public string TemplateService { get; set; }

        /// <summary>
        /// Platform Storage Service Url
        /// </summary>
        public string StorageService { get; set; }

        /// <summary>
        /// Platform Email Service Url
        /// </summary>
        public string EmailService { get; set; }

        /// <summary>
        /// Platform OrdersAPI Url
        /// </summary>
        public string OrdersApi { get; set; }

        /// <summary>
        /// Platform ServicesAPI Url
        /// </summary>
        public string ServicesApi { get; set; }

        /// <summary>
        /// Platform PartiesAPI Url
        /// </summary>
        public string PartiesApi { get; set; }

        /// <summary>
        /// Platform OrderWaiterAPI Url
        /// </summary>
        public string OrderWaiterApi { get; set; }

    }
}
