using System.ComponentModel.DataAnnotations;
using System.Linq;
using InfoTrack.Cdd.Application.Common.Attributes;

namespace InfoTrack.Cdd.Application.Common.Enums
{
    /// <summary>
    /// Service identifier (identifies which report should be ordered)
    /// </summary>
    public enum ServiceIdentifier
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,

        /// <summary>
        /// CddOrganisationReport
        /// </summary>
        /// <remarks>
        /// Company report from Frankies ("BvD replacement")
        /// </remarks>
        [Service(Description = "International Company Report")]
        CddOrganisationReport,

        /// <summary>
        /// CddPersonRiskLookup
        /// </summary>
        /// <remarks>
        /// AML Individual/Persons lookup data from Frankies ("DowJones replacement")
        /// </remarks>
        [Service(Description = "Risk & Compliance Lookup")]
        CddPersonRiskLookup,

        /// <summary>
        /// CddPersonRiskReport
        /// </summary>
        /// <remarks>
        /// AML Individual/Persons report from Frankies ("DowJones replacement")
        /// </remarks>
        [Service(Description = "Risk & Compliance Report")]
        CddPersonRiskReport
    }

    /// <summary>
    /// Service identifier enum extensions
    /// </summary>
    public static class ServiceIdentifierExtensions
    {
        public static string Description(this ServiceIdentifier self) => self.GetServiceDetails()?.Description;

        private static ServiceAttribute GetServiceDetails(this ServiceIdentifier self)
        {
            var memberInfo = typeof(ServiceIdentifier)
                .GetMember(self.ToString())
                .Single();

            if (memberInfo == null)
            {
                return null;
            }

            var attribute = (ServiceAttribute)
                memberInfo.GetCustomAttributes(typeof(ServiceAttribute), false)
                    .SingleOrDefault();
            return attribute;
        }
    }
}
