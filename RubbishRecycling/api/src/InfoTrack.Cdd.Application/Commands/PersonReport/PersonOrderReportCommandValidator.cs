using System;
using FluentValidation;
using InfoTrack.Cdd.Application.Common.Enums;

namespace InfoTrack.Cdd.Application.Commands.PersonReport
{
    /// <summary>
    /// Validate a request for detailed person info
    /// </summary>
    public class PersonOrderReportCommandValidator : AbstractValidator<PersonOrderReportCommand>
    {
        /// <summary>
        /// Validate a request for detailed person info
        /// </summary>
        public PersonOrderReportCommandValidator()
        {
            RuleFor(m => m.ServiceIdentifier)
                .NotEqual(ServiceIdentifier.Undefined)
                .Equal(ServiceIdentifier.CddPersonRiskReport);

            RuleFor(m => m.OrderId)
                .GreaterThan(0);

            RuleFor(m => m.ProviderEntityCodes)
                .NotEmpty();

            RuleForEach(m => m.ProviderEntityCodes)
                .NotEmpty();

            RuleFor(m => m.QuoteId)
                .NotEqual(Guid.Empty);
        }
    }
}
