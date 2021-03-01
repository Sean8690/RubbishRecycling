using FluentValidation;
using InfoTrack.Cdd.Application.Common.Constants;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;

namespace InfoTrack.Cdd.Application.Commands.Order
{
    /// <summary>
    /// Validate a request for detailed organisation info
    /// </summary>
    public class OrganisationOrderCommandValidator : AbstractValidator<OrganisationOrderRequestCommand>
    {
        /// <summary>
        /// Validate a request for detailed organisation info
        /// </summary>
        public OrganisationOrderCommandValidator()
        {
            RuleFor(m => m.ProviderEntityCode)
                .NotEmpty();

            RuleFor(m => m.ServiceIdentifier)
                .NotEqual(ServiceIdentifier.Undefined)
                .Equal(ServiceIdentifier.CddOrganisationReport);

            RuleFor(x => x.KyckrCountryCode)
                .NotNull()
                .Matches(Regexes.KyckrCountryCode).WithMessage($"{nameof(IOrganisation.KyckrCountryCode)} must be in valid ISO2 format or documented Kyckr format");

            RuleFor(m => m.ClientReference)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
