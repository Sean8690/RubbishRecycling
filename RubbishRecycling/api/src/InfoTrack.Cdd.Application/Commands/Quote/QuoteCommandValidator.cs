using FluentValidation;
using InfoTrack.Cdd.Application.Common.Constants;
using InfoTrack.Cdd.Application.Common.Enums;

namespace InfoTrack.Cdd.Application.Commands.Quote
{
    /// <summary>
    /// Validate request for a fee quote
    /// </summary>
    public class QuoteCommandValidator : AbstractValidator<QuoteCommand>
    {
        /// <summary>
        /// Validate request for a fee quote
        /// </summary>
        public QuoteCommandValidator()
        {
            RuleFor(m => m.ServiceIdentifier)
                .NotEqual(ServiceIdentifier.Undefined);

            RuleFor(x => x.KyckrCountryCode)
                //.NotNull() Might be required for future purpose.
                .Matches(Regexes.KyckrCountryCode).WithMessage($"{nameof(QuoteCommand.KyckrCountryCode)} must be in valid ISO2 format or documented Kyckr format");
        }
    }
}
