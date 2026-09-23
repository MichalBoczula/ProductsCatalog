using ProductCatalog.Acceptance.Tests.Features.Common;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.HealthCheckTests
{
    [Binding]
    public class HealthCheckSteps
    {
        private readonly ScenarioApiContext _apiContext;

        public HealthCheckSteps(ScenarioApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        private HttpResponseMessage? _httpResponseMessage;

        [When("I request the health endpoint")]
        public async Task WhenIRequestTheHealthEndpoint()
        {
            _httpResponseMessage = await _apiContext.Client!.GetAsync("/health/live");
        }

        [Then("the response status code should be {int}")]
        public void ThenTheResponseStatusCodeShouldBe(int p0)
        {
            _httpResponseMessage!.StatusCode.ShouldBe((System.Net.HttpStatusCode)p0);
        }
    }
}
