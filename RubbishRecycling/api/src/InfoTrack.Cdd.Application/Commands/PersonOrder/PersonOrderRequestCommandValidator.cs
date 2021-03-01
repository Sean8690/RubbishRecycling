using System;
using FluentValidation;
using InfoTrack.Cdd.Application.Common.Constants;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;

namespace InfoTrack.Cdd.Application.Commands.PersonOrder
{
    /// <summary>
    /// Validate a request for detailed person info
    /// </summary>
    public class PersonOrderRequestCommandValidator : AbstractValidator<PersonOrderRequestCommand>
    {
        /// <summary>
        /// Validate a request for detailed person info
        /// </summary>
        public PersonOrderRequestCommandValidator()
        {
            RuleFor(m => m.ServiceIdentifier)
                .NotEqual(ServiceIdentifier.Undefined)
                .Equal(ServiceIdentifier.CddPersonRiskLookup);

            RuleFor(m => m.GivenName)
                .NotEmpty();

            RuleFor(m => m.MiddleName)
                .NotEmpty().Unless(m => m.MiddleName == null); // allow null but disallow empty string

            RuleFor(m => m.FamilyName)
                .NotEmpty();

            RuleFor(m => m.ClientReference)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(m => m.DateOfBirth)
                .NotEmpty().Unless(m => m.DateOfBirth == null); // allow null but disallow empty string
            RuleFor(m => m.DateOfBirth)
                .Matches(Regexes.Date).When(m => m.DateOfBirth != null).WithMessage($"{nameof(PersonOrderRequestCommand.DateOfBirth)} must be a valid date");

            RuleFor(m => m.YearOfBirth)
                .NotEmpty().Unless(m => m.YearOfBirth == null); // allow null but disallow empty string
            RuleFor(m => m.YearOfBirth)
                .Matches(Regexes.Year).When(m => m.YearOfBirth != null).WithMessage($"{nameof(PersonOrderRequestCommand.YearOfBirth)} must be a valid date");

            // Only one or the other should be provided, not both
            RuleFor(m => m.YearOfBirth)
                .Empty().When(m => !string.IsNullOrEmpty(m.DateOfBirth));
            RuleFor(m => m.DateOfBirth)
                .Empty().When(m => !string.IsNullOrEmpty(m.YearOfBirth));

            RuleFor(m => m.QuoteId)
                .NotEqual(Guid.Empty);
        }
    }
}
