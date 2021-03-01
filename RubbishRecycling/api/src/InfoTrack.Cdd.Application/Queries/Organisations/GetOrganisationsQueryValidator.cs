using FluentValidation;
using InfoTrack.Cdd.Application.Common.Constants;
using InfoTrack.Cdd.Application.Common.Interfaces;

namespace InfoTrack.Cdd.Application.Queries.Organisations
{
    /// <summary>
    /// Validate request for getting a list of organisations matching name/number/country
    /// </summary>
    public class GetOrganisationsQueryValidator : AbstractValidator<GetOrganisationsQuery>
    {
        /// <summary>
        /// Validate request for getting a list of organisations matching name/number/country
        /// </summary>
        public GetOrganisationsQueryValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().Unless(m => !string.IsNullOrWhiteSpace(m.Number));
            RuleFor(m => m.Number)
                .NotEmpty().Unless(m => !string.IsNullOrWhiteSpace(m.Name));

            RuleFor(x => x.KyckrCountryCode)
                .NotNull()
                .Matches(Regexes.KyckrCountryCode).WithMessage($"{nameof(IOrganisation.KyckrCountryCode)} must be in valid ISO2 format or documented Kyckr format");
        }
    }
}
