using System;
using Xunit;

namespace InfoTrack.Cdd.Api.IntegrationTests
{
    /// <summary>
    /// This test should only be executed on dev.
    /// </summary>
    public sealed class DevFact : FactAttribute
    {
        public DevFact()
        {
            if (!IsDev())
            {
                Skip = "This test should only be executed on dev";
            }
        }

        private static bool IsDev() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    }

    /// <summary>
    /// This test should only be executed on dev.
    /// </summary>
    public sealed class DevTheory : TheoryAttribute
    {
        public DevTheory()
        {
            if (!IsDev())
            {
                Skip = "This test should only be executed on dev";
            }
        }

        private static bool IsDev() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    }
}
