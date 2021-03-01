using System;
using FluentValidation.TestHelper;
using InfoTrack.Cdd.Application.Commands.PersonOrder;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Dtos;
using Xunit;

#pragma warning disable CA1801 // Review unused parameters
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
// TODO is it possible to use "because" parameters?

namespace InfoTrack.Cdd.Application.UnitTests.QueriesAndCommands
{
    public class PersonOrderRequestCommandValidatorTests
    {
        private const string ValidGivenName = "John";
        private const string ValidMiddleName = "James";
        private const string ValidFamilyName = "Smith";
        private const string ValidDateOfBirth = "1990-05-04";
        private const string ValidYearOfBirth = "1990";
        private const string ValidClientReference = "test";
        private Guid? ValidQuoteId = Guid.NewGuid();
        private const ServiceIdentifier ValidServiceIdentifier = ServiceIdentifier.CddPersonRiskLookup;
        private const string ValidRetailerReference = "XXXXXRETAILERREFERENCE";

        [Fact]
        public void GivenValidInputWithQuoteId_WhenValidated_ThenNoExceptionThrown()
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                MiddleName = ValidMiddleName,
                FamilyName = ValidFamilyName,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                QuoteId = ValidQuoteId,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldNotHaveValidationErrorFor(x => x.GivenName);
            validator.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
            validator.ShouldNotHaveValidationErrorFor(x => x.FamilyName);
            validator.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.YearOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.ClientReference);
            validator.ShouldNotHaveValidationErrorFor(x => x.ServiceIdentifier);
            validator.ShouldNotHaveValidationErrorFor(x => x.QuoteId);
            validator.ShouldNotHaveValidationErrorFor(x => x.RetailerReference);
        }

        [Fact]
        public void GivenValidInputWithNullQuoteId_WhenValidated_ThenNoExceptionThrown()
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                MiddleName = ValidMiddleName,
                FamilyName = ValidFamilyName,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                QuoteId = null,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldNotHaveValidationErrorFor(x => x.GivenName);
            validator.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
            validator.ShouldNotHaveValidationErrorFor(x => x.FamilyName);
            validator.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.YearOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.ClientReference);
            validator.ShouldNotHaveValidationErrorFor(x => x.ServiceIdentifier);
            validator.ShouldNotHaveValidationErrorFor(x => x.QuoteId);
            validator.ShouldNotHaveValidationErrorFor(x => x.RetailerReference);
        }

        [Fact]
        public void GivenValidInputWithDateOfBirth_WhenValidated_ThenNoExceptionThrown()
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                MiddleName = ValidMiddleName,
                FamilyName = ValidFamilyName,
                DateOfBirth = ValidDateOfBirth,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                QuoteId = ValidQuoteId,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldNotHaveValidationErrorFor(x => x.GivenName);
            validator.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
            validator.ShouldNotHaveValidationErrorFor(x => x.FamilyName);
            validator.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.YearOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.ClientReference);
            validator.ShouldNotHaveValidationErrorFor(x => x.ServiceIdentifier);
            validator.ShouldNotHaveValidationErrorFor(x => x.QuoteId);
            validator.ShouldNotHaveValidationErrorFor(x => x.RetailerReference);
        }

        [Fact]
        public void GivenValidInputWithoutMiddleName_WhenValidated_ThenNoExceptionThrown()
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                FamilyName = ValidFamilyName,
                DateOfBirth = ValidDateOfBirth,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                QuoteId = ValidQuoteId,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldNotHaveValidationErrorFor(x => x.GivenName);
            validator.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
            validator.ShouldNotHaveValidationErrorFor(x => x.FamilyName);
            validator.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.YearOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.ClientReference);
            validator.ShouldNotHaveValidationErrorFor(x => x.ServiceIdentifier);
            validator.ShouldNotHaveValidationErrorFor(x => x.QuoteId);
            validator.ShouldNotHaveValidationErrorFor(x => x.RetailerReference);
        }

        [Fact]
        public void GivenValidInputWithYearOfBirth_WhenValidated_ThenNoExceptionThrown()
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                MiddleName = ValidMiddleName,
                FamilyName = ValidFamilyName,
                YearOfBirth = ValidYearOfBirth,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                QuoteId = ValidQuoteId,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldNotHaveValidationErrorFor(x => x.GivenName);
            validator.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
            validator.ShouldNotHaveValidationErrorFor(x => x.FamilyName);
            validator.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.YearOfBirth);
            validator.ShouldNotHaveValidationErrorFor(x => x.ClientReference);
            validator.ShouldNotHaveValidationErrorFor(x => x.ServiceIdentifier);
            validator.ShouldNotHaveValidationErrorFor(x => x.QuoteId);
            validator.ShouldNotHaveValidationErrorFor(x => x.RetailerReference);
        }


        [Theory]
        [InlineData(ServiceIdentifier.CddOrganisationReport)]
        [InlineData(ServiceIdentifier.Undefined)]
        [InlineData(ServiceIdentifier.CddPersonRiskReport)]
        public void GivenInvalidServiceIdentifier_WhenValidated_ThenExceptionThrown(ServiceIdentifier serviceIdentifier)
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                MiddleName = ValidMiddleName,
                FamilyName = ValidFamilyName,
                ClientReference = ValidClientReference,
                ServiceIdentifier = serviceIdentifier,
                QuoteId = ValidQuoteId,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.ServiceIdentifier);
        }

        [Fact]
        public void GivenDateAndYearOfBirth_WhenValidated_ThenExceptionThrown()
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                MiddleName = ValidMiddleName,
                FamilyName = ValidFamilyName,
                DateOfBirth = ValidDateOfBirth,
                YearOfBirth = ValidYearOfBirth,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                QuoteId = ValidQuoteId,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
            validator.ShouldHaveValidationErrorFor(x => x.YearOfBirth);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("5 December 2000")]
        [InlineData("5/12/2000")]
        [InlineData("5-12-2000")]
        [InlineData("20-12-05")]
        public void GivenInvalidDateOfBirth_WhenValidated_ThenExceptionThrown(string dateOfBirth)
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                FamilyName = ValidFamilyName,
                DateOfBirth = dateOfBirth,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("20")]
        [InlineData("2000-01-01")]
        public void GivenInvalidYearOfBirth_WhenValidated_ThenExceptionThrown(string yearOfBirth)
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                FamilyName = ValidFamilyName,
                YearOfBirth = yearOfBirth,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.YearOfBirth);
        }

        [Theory]
        [InlineData(null, null, null, true, false, true)]
        [InlineData("", "", "", true, true, true)]
        [InlineData(" ", " ", " ", true, true, true)]
        [InlineData(null, "Fred", "Smith", true, false, false)]
        [InlineData(null, "F", null, true, false, true)]
        public void GivenInvalidName_WhenValidated_ThenExceptionThrown(string givenName, string middleName, string familyName,
            bool givenNameError, bool middleNameError, bool familyNameError)
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = givenName,
                MiddleName = middleName,
                FamilyName = familyName,
                DateOfBirth = ValidDateOfBirth,
                YearOfBirth = ValidYearOfBirth,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                QuoteId = ValidQuoteId,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            if (givenNameError)
            {
                validator.ShouldHaveValidationErrorFor(x => x.GivenName);
            }
            if (middleNameError)
            {
                validator.ShouldHaveValidationErrorFor(x => x.MiddleName);
            }
            if (familyNameError)
            {
                validator.ShouldHaveValidationErrorFor(x => x.FamilyName);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("referencehaslengthof51_xxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
        public void GivenInvalidClientReference_WhenValidated_ThenExceptionThrown(string clientReference)
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                MiddleName = ValidMiddleName,
                FamilyName = ValidFamilyName,
                ClientReference = clientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.ClientReference);
        }

        [Fact]
        public void GivenInvalidQuoteId_WhenValidated_ThenExceptionThrown()
        {
            var request = new AmlPersonLookupRequest
            {
                GivenName = ValidGivenName,
                MiddleName = ValidMiddleName,
                FamilyName = ValidFamilyName,
                ClientReference = ValidClientReference,
                ServiceIdentifier = ValidServiceIdentifier,
                QuoteId = Guid.Empty,
                RetailerReference = ValidRetailerReference
            };
            var query = new PersonOrderRequestCommand(request);
            var validator = new PersonOrderRequestCommandValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.QuoteId);
        }

    }
}
