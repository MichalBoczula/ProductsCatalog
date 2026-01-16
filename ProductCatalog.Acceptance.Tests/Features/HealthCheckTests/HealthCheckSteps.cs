using ProductCatalog.Acceptance.Tests.Features.Common;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.HealthCheckTests
{
    [Binding]
    public class HealthCheckSteps
    {
        private HttpResponseMessage _httpResponseMessage;

        [When("I request the health endpoint")]
        public async Task WhenIRequestTheHealthEndpoint()
        {
            _httpResponseMessage = await TestRunHooks.Client.GetAsync("/health");
        }

        [Then("the response status code should be {int}")]
        public void ThenTheResponseStatusCodeShouldBe(int p0)
        {
            _httpResponseMessage.StatusCode.ShouldBe((System.Net.HttpStatusCode)p0);
        }
    }
}
