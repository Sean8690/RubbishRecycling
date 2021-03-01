using System;
using Xunit;

namespace InfoTrack.Cdd.Api.IntegrationTests.Controllers
{
    public abstract class ControllerTestBase : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        protected internal readonly CustomWebApplicationFactory<Startup> _factory;

        protected ControllerTestBase(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
    }
}
