using System.Net;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.Products;

[Binding]
public class SharedProductsSteps
{
    private readonly ScenarioContext _scenarioContext;

    public SharedProductsSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Then("the response status code should be (.*)")]
    public void ThenTheResponseStatusCodeShouldBe(int statusCode)
    {
        var response = _scenarioContext.TryGetValue("response", out HttpResponseMessage? httpResponse)
            ? httpResponse
            : null;

        response.ShouldNotBeNull();
        ((int)response.StatusCode).ShouldBe(statusCode);
    }
}
