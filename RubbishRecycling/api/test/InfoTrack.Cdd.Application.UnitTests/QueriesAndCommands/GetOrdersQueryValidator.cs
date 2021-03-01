using FluentValidation.TestHelper;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Queries.Orders;
using Xunit;

#pragma warning disable CA1801 // Review unused parameters
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
// TODO is it possible to use "because" parameters?

namespace InfoTrack.Cdd.Application.UnitTests.QueriesAndCommands
{
    public class GetOrdersQueryValidatorTests
    {
        private const string ValidCountryIso2 = "AU";
        private const string ValidProviderEntityCode = "XXXX123";
        private const string ValidClientReference = "test";
        private const string ValidRetailerReference = "XXXXXRETAILERREFERENCE";

        [Theory]
        [InlineData(ServiceIdentifier.CddOrganisationReport, ValidProviderEntityCode, "AU-SA", "t", "XXXXXXXXXXX")]
        [InlineData(ServiceIdentifier.CddPersonRiskLookup, ValidProviderEntityCode, ValidCountryIso2, ValidClientReference, ValidRetailerReference)]
        [InlineData(ServiceIdentifier.CddPersonRiskReport, ValidProviderEntityCode, ValidCountryIso2, "referencehasmaxlengthof50_xxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
        public void GivenValidInput_WhenValidated_ThenNoExceptionThrown(ServiceIdentifier serviceIdentifier, string providerEntityCode, string kyckrCountryCode, string clientReference, string retailerReference)
        {
            var query = new GetOrdersQuery(serviceIdentifier, providerEntityCode, kyckrCountryCode, clientReference, retailerReference);
            var validator = new GetOrdersQueryValidator().TestValidate(query);

            validator.ShouldNotHaveValidationErrorFor(x => x.ProviderEntityCode);
            validator.ShouldNotHaveValidationErrorFor(x => x.ServiceIdentifier);
            validator.ShouldNotHaveValidationErrorFor(x => x.KyckrCountryCode);
            validator.ShouldNotHaveValidationErrorFor(x => x.ClientReference);
            validator.ShouldNotHaveValidationErrorFor(x => x.RetailerReference);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GivenInvalidProviderEntityCode_WhenValidated_ThenExceptionThrown(string providerEntityCode)
        {
            var query = new GetOrdersQuery(ServiceIdentifier.CddPersonRiskLookup, providerEntityCode, ValidCountryIso2, ValidClientReference, ValidRetailerReference);
            var validator = new GetOrdersQueryValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.ProviderEntityCode);
        }

        [Fact]
        public void GivenInvalidServiceIdentifier_WhenValidated_ThenExceptionThrown()
        {
            var query = new GetOrdersQuery(ServiceIdentifier.Undefined, ValidProviderEntityCode, ValidCountryIso2, ValidClientReference, ValidRetailerReference);
            var validator = new GetOrdersQueryValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.ServiceIdentifier);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("referencehaslengthof51_xxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
        public void GivenInvalidClientReference_WhenValidated_ThenExceptionThrown(string clientReference)
        {
            var query = new GetOrdersQuery(ServiceIdentifier.CddPersonRiskLookup, ValidProviderEntityCode, ValidCountryIso2, clientReference, ValidRetailerReference);
            var validator = new GetOrdersQueryValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.ClientReference);
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
            var query = new GetOrdersQuery(ServiceIdentifier.CddOrganisationReport, ValidProviderEntityCode, country, ValidClientReference, ValidRetailerReference);
            var validator = new GetOrdersQueryValidator().TestValidate(query);

            validator.ShouldHaveValidationErrorFor(x => x.KyckrCountryCode);
        }
    }
}
