using FluentValidation.TestHelper;
using InfoTrack.Cdd.Application.Queries.Organisations;
using Xunit;

#pragma warning disable CA1801 // Review unused parameters
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
// TODO is it possible to use "because" parameters?

namespace InfoTrack.Cdd.Application.UnitTests.QueriesAndCommands
{
    public class GetOrganisationQueryValidatorTests
    {
        private const string ValidName = "InfoTrack Pty Limited";
        private const string ValidNumber = "36 092 724 251";
        private const string ValidCountryIso2 = "AU";

        [Fact]
        public void GivenValidNameAndNumberAndCountry_WhenValidated_ThenNoExceptionThrown()
        {
            var query = new GetOrganisationsQuery(ValidName, ValidNumber, ValidCountryIso2);
            var validator = new GetOrganisationsQueryValidator().TestValidate(query);
            
            validator.ShouldNotHaveValidationErrorFor(x => x.Name);
            validator.ShouldNotHaveValidationErrorFor(x => x.Number);
            validator.ShouldNotHaveValidationErrorFor(x => x.KyckrCountryCode);
        }

        [Theory]
        [InlineData(null, "Null name is valid if number is provided")]
        [InlineData("", "Empty name is valid if number is provided")]
        [InlineData(" ", "Whitespace name is valid if number is provided")]
        public void GivenValidNumberAndCountryOnly_WhenValidated_ThenNoExceptionThrown(string name, string because)
        {
            var query = new GetOrganisationsQuery(name, ValidNumber, ValidCountryIso2);
            var validator = new GetOrganisationsQueryValidator().TestValidate(query);
            
            validator.ShouldNotHaveValidationErrorFor(x => x.Name);
            validator.ShouldNotHaveValidationErrorFor(x => x.Number);
            validator.ShouldNotHaveValidationErrorFor(x => x.KyckrCountryCode);
        }

        [Theory]
        [InlineData(null, "Null number is valid if name is provided")]
        [InlineData("", "Empty number is valid if name is provided")]
        [InlineData(" ", "Whitespace number is valid if name is provided")]
        public void GivenValidNameAndCountryOnly_WhenValidated_ThenNoExceptionThrown(string number, string because)
        {
            var query = new GetOrganisationsQuery(ValidName, number, ValidCountryIso2);
            var validator = new GetOrganisationsQueryValidator().TestValidate(query);
            
            validator.ShouldNotHaveValidationErrorFor(x => x.Name);
            validator.ShouldNotHaveValidationErrorFor(x => x.Number);
            validator.ShouldNotHaveValidationErrorFor(x => x.KyckrCountryCode);
        }

        [Theory]
        [InlineData("AU")]
        [InlineData("AU-SA")]
        public void GivenValidCountry_WhenValidated_ThenNoExceptionThrown(string country)
        {
            var query = new GetOrganisationsQuery(ValidName, ValidNumber, country);
            var validator = new GetOrganisationsQueryValidator().TestValidate(query);
            
            validator.ShouldNotHaveValidationErrorFor(x => x.KyckrCountryCode);
        }

        [Theory]
        [InlineData(null, "Country is required")]
        [InlineData("", "Country is required")]
        [InlineData(" ", "Country is required")]
        [InlineData("A", "Country must be at least 2 chars")]
        [InlineData("aa", "Country must be in expected uppercase")]
        [InlineData("AA-", "Country must be valid")]
        [InlineData("AA-A", "Country must be valid")]
        [InlineData("AA-AAA", "Country must be valid")]
        public void GivenInvalidCountry_WhenValidated_ThenExceptionThrown(string country, string because)
        {
            var query = new GetOrganisationsQuery(ValidName, ValidNumber, country);
            var validator = new GetOrganisationsQueryValidator().TestValidate(query);
            
            validator.ShouldHaveValidationErrorFor(x => x.KyckrCountryCode);
        }

        [Theory]
        [InlineData(null, null, "Name and/or Number is required")]
        [InlineData("", "", "Name and/or Number is required")]
        [InlineData(" ", " ", "Name and/or Number is required")]
        [InlineData(null, " ", "Name and/or Number is required")]
        public void GivenInvalidNameAndNumber_WhenValidated_ThenExceptionThrown(string name, string number, string because)
        {
            var query = new GetOrganisationsQuery(name, number, ValidCountryIso2);
            var validator = new GetOrganisationsQueryValidator().TestValidate(query);
            
            validator.ShouldHaveValidationErrorFor(x => x.Name);
            validator.ShouldHaveValidationErrorFor(x => x.Number);
        }

      
    }
}
