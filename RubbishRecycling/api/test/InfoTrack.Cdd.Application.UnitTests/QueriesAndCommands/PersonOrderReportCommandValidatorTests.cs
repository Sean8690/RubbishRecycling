using System;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using InfoTrack.Cdd.Application.Commands.PersonOrder;
using InfoTrack.Cdd.Application.Commands.PersonReport;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Queries.Organisations;
using Xunit;

#pragma warning disable CA1801 // Review unused parameters
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
// TODO is it possible to use "because" parameters?

namespace InfoTrack.Cdd.Application.UnitTests.QueriesAndCommands
{
    public class PersonOrderReportCommandValidatorTests
    {
        private const int ValidOrderId = 1234567;
        private const string ValidProviderEntityCode = "XXX123";
        private Guid? ValidQuoteId = Guid.NewGuid();
        private const ServiceIdentifier ValidServiceIdentifier = ServiceIdentifier.CddPersonRiskReport;

        [Fact]
        public void GivenValidInputWithQuoteId_WhenValidated_ThenNoExceptionThrown()
        {
            var query = new PersonOrderReportCommand(ValidOrderId, new List<string> { ValidProviderEntityCode }, ValidServiceIdentifier, ValidQuoteId);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);
            
            validator.ShouldNotHaveValidationErrorFor(x => x.ServiceIdentifier);
            validator.ShouldNotHaveValidationErrorFor(x => x.OrderId);
            validator.ShouldNotHaveValidationErrorFor(x => x.ProviderEntityCodes);
            validator.ShouldNotHaveValidationErrorFor(x => x.QuoteId);
        }

        [Fact]
        public void GivenValidInputWithNullQuoteId_WhenValidated_ThenNoExceptionThrown()
        {
            var query = new PersonOrderReportCommand(ValidOrderId, new List<string> { ValidProviderEntityCode }, ValidServiceIdentifier, null);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);

            validator.ShouldNotHaveValidationErrorFor(x => x.ServiceIdentifier);
            validator.ShouldNotHaveValidationErrorFor(x => x.OrderId);
            validator.ShouldNotHaveValidationErrorFor(x => x.ProviderEntityCodes);
            validator.ShouldNotHaveValidationErrorFor(x => x.QuoteId);
        }

        [Fact]
        public void GivenValidInputWithMultipleProviderEntityCodes_WhenValidated_ThenNoExceptionThrown()
        {
            var query = new PersonOrderReportCommand(ValidOrderId, new List<string> { ValidProviderEntityCode, "123456", "AABBCC" }, ValidServiceIdentifier, null);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);

            validator.ShouldNotHaveValidationErrorFor(x => x.ServiceIdentifier);
            validator.ShouldNotHaveValidationErrorFor(x => x.OrderId);
            validator.ShouldNotHaveValidationErrorFor(x => x.ProviderEntityCodes);
            validator.ShouldNotHaveValidationErrorFor(x => x.QuoteId);
        }


        [Theory]
        [InlineData(ServiceIdentifier.CddOrganisationReport)]
        [InlineData(ServiceIdentifier.Undefined)]
        [InlineData(ServiceIdentifier.CddPersonRiskLookup)]
        public void GivenInvalidServiceIdentifier_WhenValidated_ThenExceptionThrown(ServiceIdentifier serviceIdentifier)
        {
            var query = new PersonOrderReportCommand(ValidOrderId, new List<string> { ValidProviderEntityCode }, serviceIdentifier, ValidQuoteId);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);
            
            validator.ShouldHaveValidationErrorFor(x => x.ServiceIdentifier);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-999)]
        public void GivenInvalidOrderId_WhenValidated_ThenExceptionThrown(int orderId)
        {
            var query = new PersonOrderReportCommand(orderId, new List<string> { ValidProviderEntityCode }, ValidServiceIdentifier, ValidQuoteId);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.OrderId);
        }

        [Fact]
        public void GivenInvalidQuoteId_WhenValidated_ThenExceptionThrown()
        {
            var query = new PersonOrderReportCommand(ValidOrderId, new List<string> { ValidProviderEntityCode }, ValidServiceIdentifier, Guid.Empty);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.QuoteId);
        }

        [Fact]
        public void GivenEmptyProviderEntityCodes_WhenValidated_ThenExceptionThrown()
        {
            var query = new PersonOrderReportCommand(ValidOrderId, new List<string>(), ValidServiceIdentifier, ValidQuoteId);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.ProviderEntityCodes);
        }

        [Fact]
        public void GivenNullProviderEntityCodes_WhenValidated_ThenExceptionThrown()
        {
            var query = new PersonOrderReportCommand(ValidOrderId, null, ValidServiceIdentifier, ValidQuoteId);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.ProviderEntityCodes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GivenInvalidProviderEntityCodes_WhenValidated_ThenExceptionThrown(string providerEntityCode)
        {
            var query = new PersonOrderReportCommand(ValidOrderId, new List<string> { providerEntityCode }, ValidServiceIdentifier, ValidQuoteId);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.ProviderEntityCodes);
        }

        [Fact(Skip ="TODO consider if this is a requirement")]
        public void GivenDuplicatedProviderEntityCodes_WhenValidated_ThenExceptionThrown()
        {
            var query = new PersonOrderReportCommand(ValidOrderId, new List<string> { ValidProviderEntityCode, ValidProviderEntityCode }, ValidServiceIdentifier, ValidQuoteId);
            var validator = new PersonOrderReportCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.ProviderEntityCodes);
        }


    }
}
