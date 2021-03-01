using FluentValidation;
using InfoTrack.Cdd.Application.Common.Constants;
using InfoTrack.Cdd.Application.Common.Enums;

namespace InfoTrack.Cdd.Application.Queries.Orders
{
    /// <summary>
    /// Validate request for getting a list of orders matching serviceIdentifier/providerEntityCode/kyckrCountryCode/clientReference
    /// </summary>
    public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
    {
        /// <summary>
        /// Validate request for getting a list of orders matching serviceIdentifier/providerEntityCode/kyckrCountryCode/clientReference
        /// </summary>
        public GetOrdersQueryValidator()
        {
            RuleFor(m => m.ProviderEntityCode)
                .NotEmpty();

            RuleFor(m => m.ServiceIdentifier)
                .NotEqual(ServiceIdentifier.Undefined);

            RuleFor(x => x.KyckrCountryCode)
                .NotNull()
                .Matches(Regexes.KyckrCountryCode).WithMessage($"{nameof(GetOrdersQuery.KyckrCountryCode)} must be in valid ISO2 format or documented Kyckr format");

            RuleFor(m => m.ClientReference)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
