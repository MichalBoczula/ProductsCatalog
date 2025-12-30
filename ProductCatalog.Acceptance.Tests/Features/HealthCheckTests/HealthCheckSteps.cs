using ProductCatalog.Acceptance.Tests.Features.Common;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features
{
    [Binding]
    public class HealthCheckSteps
    {
        private HttpResponseMessage? _response;

        [When("I request the health endpoint")]
        public async Task WhenIRequestTheHealthEndpoint()
        {
            _response = await TestRunHooks.Client.GetAsync("/health");
        }

        [Then("the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            _response.ShouldNotBeNull();
            ((int)_response.StatusCode).ShouldBe(statusCode);
        }
    }
}
