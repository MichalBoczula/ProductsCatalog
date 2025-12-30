using ProductCatalog.Acceptance.Tests.Features.Common;
using Reqnroll;

namespace ProductCatalog.Acceptance.Tests.Features
{
    [Binding]
    public class HealthCheckSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public HealthCheckSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When("I request the health endpoint")]
        public async Task WhenIRequestTheHealthEndpoint()
        {
            var response = await TestRunHooks.Client.GetAsync("/health");
            _scenarioContext["response"] = response;
        }
    }
}
