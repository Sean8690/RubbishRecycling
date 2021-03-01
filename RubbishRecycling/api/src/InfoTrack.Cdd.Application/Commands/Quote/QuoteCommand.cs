using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Dtos;
using InfoTrack.Common.Application;

namespace InfoTrack.Cdd.Application.Commands.Quote
{
    /// <summary>
    /// Get a fee quote
    /// </summary>
    public class QuoteCommand : ICommand<QuoteDto>
    {
        /// <summary>
        /// Get a fee quote
        /// </summary>
        public QuoteCommand(ServiceIdentifier serviceIdentifier, string kyckrCountryCode)
        {
            ServiceIdentifier = serviceIdentifier;
            KyckrCountryCode = kyckrCountryCode;
        }

        /// <summary>
        /// Service identifier (identifies which report should be ordered)
        /// </summary>
        public ServiceIdentifier ServiceIdentifier { get; set; }

        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        public string KyckrCountryCode { get; set; }
    }
}
