using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
using InfoTrack.Cdd.Application.Common.Enums;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;
using InfoTrack.Cdd.Infrastructure.Models.Frankie;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions;
using InfoTrack.Cdd.Infrastructure.Models.Frankie.PersonSearch;
using InfoTrack.Cdd.Infrastructure.Templates;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace InfoTrack.Cdd.Application.UnitTests.Templates
{
    public class TemplateBuilderTests
    {
        private readonly ITemplateBuilder _templateBuilder;

        public TemplateBuilderTests()
        {
            var loggerMock = new Mock<ILogger<TemplateBuilder>>();

            _templateBuilder = new TemplateBuilder(loggerMock.Object);
        }

        [Theory]
        [InlineData("InfoTrack_AU")]
        [InlineData("InfoTrack_GB")]
        [InlineData("InfoTrack_NZ")]
        [InlineData("InfoTrack_US-CA")]
        [InlineData("3AComposites_DE")]
        [InlineData("Haribo_LU")]
        [InlineData("LogosAustralia_SG")]
        public async void Build_CddOrganisationReport_ReturnsResponse(string fileName)
        {
            var result = await _templateBuilder.Build(GetSampleInternationalBusinessProfileResponse(fileName), ServiceIdentifier.CddOrganisationReport.ToString());

            Directory.CreateDirectory("Output");
            File.WriteAllText($"./Output/{nameof(Build_CddOrganisationReport_ReturnsResponse)}_{fileName}.html", result);

            result.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("hillaryclinton")]
        [InlineData("kimjongun")]
        [InlineData("donaldtrump")]
        [InlineData("osamaBinLaden")]
        [InlineData("clear")]
        public async void Build_CddPersonRiskReport_ReturnsResponse(string fileName)
        {
            var result = await _templateBuilder.Build(GetSamplePersonSearchResponse(fileName), ServiceIdentifier.CddPersonRiskReport.ToString());

            Directory.CreateDirectory("Output");
            File.WriteAllText($"./Output/{nameof(Build_CddPersonRiskReport_ReturnsResponse)}_{fileName}.html", result);

            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void Build_CddOrganisationReport_ThrowsExceptionOnNullModel()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _templateBuilder.Build<InternationalBusinessProfileResponse>(null, ServiceIdentifier.CddOrganisationReport.ToString()));
        }

        [Fact]
        public async void Build_ThrowsExceptionOnNullTemplate()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _templateBuilder.Build(new InternationalBusinessProfileResponse(), null));
        }

        private static TemplateModel<InternationalBusinessProfileResponse> GetSampleInternationalBusinessProfileResponse(string fileName)
        {
            var input = File.ReadAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Data/InternationalBusinessProfileResponse/{fileName}.json", Encoding.UTF8);
            var result = JsonConvert.DeserializeObject<InternationalBusinessProfileResponse>(input);
            result.KyckrCountryCode = fileName.Split("_")[1];
            result.CountryName = $"Australia ({result.KyckrCountryCode} Unit Test)";
            result.CountryFlag = new Uri("https://storage.infotrack.com.au/v2/files/ee4be860b9774b59b01817feddb258acd89c69725ab91137db4a044ddae7c429/AU.png");
            return new TemplateModel<InternationalBusinessProfileResponse>(result) { ReportTitle = ServiceIdentifier.CddOrganisationReport.Description() };
        }

        private static TemplateModel<FrankieSearchResults> GetSamplePersonSearchResponse(string fileName)
        {
            var input = File.ReadAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Data/PersonSearchResponse/{fileName}.json", Encoding.UTF8);
            var result = JsonConvert.DeserializeObject<CheckEntityCheckResultObject>(input);
            return new TemplateModel<FrankieSearchResults>(result.TransformAuthorityResponse(new PersonSearchCriteria.EntityRequest
            {
                Entity = new PersonSearchCriteria.Entity
                {
                    Name = new PersonSearchCriteria.Name
                    {
                        GivenName = "Joseph",
                        FamilyName = "Bloggs"
                    },
                    DateOfBirth = new PersonSearchCriteria.DateOfBirth
                    {
                        YearOfBirth = "1994"
                    }
                }
            }).PersonResults.First())
            { ReportTitle = ServiceIdentifier.CddPersonRiskReport.Description() };
        }
    }
}
