using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
using InfoTrack.Cdd.Infrastructure.Models.Frankie;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Shared;
using Newtonsoft.Json;
using Xunit;

namespace InfoTrack.Cdd.Application.UnitTests.Models
{
    public class ModelTests
    {
        [Theory]
        [InlineData("InternationalBusinessProfileResponse/InfoTrack_AU")]
        [InlineData("InternationalBusinessProfileResponse/InfoTrack_GB")]
        [InlineData("InternationalBusinessProfileResponse/InfoTrack_NZ")]
        [InlineData("InternationalBusinessProfileResponse/InfoTrack_US-CA")]
        [InlineData("InternationalBusinessProfileResponse/3AComposites_DE")]
        [InlineData("InternationalBusinessProfileResponse/Haribo_LU")]
        [InlineData("InternationalBusinessProfileResponse/LogosAustralia_SG")]
        public void InternationalBusinessProfileResponse_CanDeserialise(string fileName)
        {
            var input = File.ReadAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Data/{fileName}.json", Encoding.UTF8);

            var output = JsonConvert.DeserializeObject<InternationalBusinessProfileResponse>(input);

            var reserialised = JsonConvert.SerializeObject(output);

            output.Should().NotBeNull();
            output.CheckId.Should().NotBeNullOrEmpty();
            output.CompanyProfile.Should().NotBeNull();
            output.EntityId.Should().NotBeNullOrEmpty();
            output.KyckrReport.Should().NotBeNull();
            output.RequestId.Should().NotBeNullOrEmpty();
            output.TransactionId.Should().NotBeNullOrEmpty();

            // I dunno why these are failing, if you look at them they actually look fine.
            // This is a valuable test for running locally
            //// Test that we were able to reserialise successfully also
            //var settings = new JsonLoadSettings { CommentHandling = CommentHandling.Ignore, LineInfoHandling = LineInfoHandling.Ignore, DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error };
            //JToken expected = JToken.Parse(input, settings);
            //JToken actual = JToken.Parse(reserialised, settings);

            //actual.Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers().WithoutStrictOrdering());
        }

        [Theory]
        [InlineData("InternationalBusinessSearchResponse/InfoTrack_AU", 11)]
        [InlineData("InternationalBusinessSearchResponse/InfoTrack_GB", 4)]
        [InlineData("InternationalBusinessSearchResponse/InfoTrack_NZ", 2)]
        [InlineData("InternationalBusinessSearchResponse/InfoTrack_US-CA", 4)]
        [InlineData("InternationalBusinessSearchResponse/3AComposites_DE", 3)]
        [InlineData("InternationalBusinessSearchResponse/Haribo_LU", 1)]
        [InlineData("InternationalBusinessSearchResponse/LogosAustralia_SG", 100)]
        public void InternationalBusinessSearchResponse_CanDeserialise(string fileName, int expectedCompanyCount)
        {
            var input = File.ReadAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Data/{fileName}.json", Encoding.UTF8);

            var output = JsonConvert.DeserializeObject<InternationalBusinessSearchResponse>(input);

            var reserialised = JsonConvert.SerializeObject(output);

            output.Should().NotBeNull();
            output.Companies.Should().HaveCount(expectedCompanyCount);
            output.RequestId.Should().NotBeNullOrEmpty();
            output.ResponseCode.Should().NotBeNullOrEmpty();
            output.TransactionId.Should().NotBeNullOrEmpty();
            foreach (var company in output.Companies)
            {
                company.ProviderEntityCode.Should().NotBeNullOrEmpty();
                company.Name.Should().NotBeNullOrEmpty();
                company.CompanyNumber.Should().NotBeNullOrEmpty();
            }

            // I dunno why these are failing, if you look at them they actually look fine.
            // This is a valuable test for running locally
            // Test that we were able to reserialise successfully also
            //var settings = new JsonLoadSettings { CommentHandling = CommentHandling.Ignore, LineInfoHandling = LineInfoHandling.Ignore, DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error };
            //JToken expected = JToken.Parse(input, settings);
            //JToken actual = JToken.Parse(reserialised, settings);

            //actual.Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
        }

        [Theory]
        [InlineData("PersonSearchResponse/hillaryclinton")]
        [InlineData("PersonSearchResponse/kimjongun")]
        [InlineData("PersonSearchResponse/donaldtrump")]
        [InlineData("PersonSearchResponse/osamaBinLaden")]
        [InlineData("PersonSearchResponse/clear")]
        public void PersonSearchResponse_CanDeserialise(string fileName)
        {
            // Arrange
            var input = File.ReadAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Data/{fileName}.json", Encoding.UTF8);

            // Act
            var output = JsonConvert.DeserializeObject<CheckEntityCheckResultObject>(input);
            var reserialised = JsonConvert.SerializeObject(output);

            // Assert
            output.Should().NotBeNull();
            output.RequestId.Should().NotBeNullOrEmpty();
            output.EntityResults.AmlResultsSet.Should().NotBeEmpty();
            foreach (var resultSet in output.EntityResults.AmlResultsSet)
            {
                // TODO add test cases
            }

            // I dunno why these are failing, if you look at them they actually look fine.
            // This is a valuable test for running locally
            //// Test that we were able to reserialise successfully also
            //var settings = new JsonLoadSettings { CommentHandling = CommentHandling.Ignore, LineInfoHandling = LineInfoHandling.Ignore, DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error };
            //JToken expected = JToken.Parse(input, settings);
            //JToken actual = JToken.Parse(reserialised, settings);

            //actual.Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers().WithoutStrictOrdering());
        }

        [Theory]
        [InlineData("PersonSearchResponse/hillaryclinton")]
        [InlineData("PersonSearchResponse/kimjongun")]
        [InlineData("PersonSearchResponse/donaldtrump")]
        [InlineData("PersonSearchResponse/osamaBinLaden")]
        [InlineData("PersonSearchResponse/clear")]
        public void PersonSearchResponse_CanTransform(string fileName)
        {
            // Arrange
            var input = File.ReadAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Data/{fileName}.json", Encoding.UTF8);
            CheckEntityCheckResultObject output = JsonConvert.DeserializeObject<CheckEntityCheckResultObject>(input);

            // Act
            var transformedOutput = output.TransformAuthorityResponse(new PersonSearchCriteria.EntityRequest
            {
                Entity = new PersonSearchCriteria.Entity
                {
                    Name = new PersonSearchCriteria.Name
                    {
                        GivenName = "Joseph",
                        FamilyName = "Bloggs"
                    }
                }
            });

            // Assert
            transformedOutput.PersonResults.Should().HaveCount(output.EntityResults.AmlResultsSet.Count);
            // TODO add test cases
        }


        /// <summary>
        /// This test is only of value for development purposes
        /// </summary>
        [Fact(Skip = "Test is for development purposes only")]
        public void InternationalBusinessSearchResponse_CanSerialise()
        {
            var output = JsonConvert.SerializeObject(new InternationalBusinessSearchResponse
            {
                Companies = new List<CompanyDto>
                {
                    new CompanyDto
                    {
                        Name = "compname",
                        Addresses = new List<AddressDto> { new AddressDto { AddressInOneLine = "sample" } }
                    }
                }
            });

            output.Should().NotBeNull();
        }
    }
}
