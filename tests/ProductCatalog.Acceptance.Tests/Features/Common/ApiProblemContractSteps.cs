using System.Net;
using System.Text;
using System.Text.Json;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

[Binding]
public sealed class ApiProblemContractSteps
{
    private HttpResponseMessage? _response;
    private string? _path;
    private string? _errorCase;

    [When("I trigger the Products REF-06 error case {string}")]
    public async Task WhenITriggerTheErrorCase(string errorCase)
    {
        var client = TestRunHooks.Client;
        _errorCase = errorCase;
        (_path, _response) = errorCase switch
        {
            "route" => ("/ref-06-not-found", await client.GetAsync("/ref-06-not-found")),
            "method" => ("/mobile-phones/top", await client.PostAsync("/mobile-phones/top", null)),
            "media" => ("/mobile-phones", await client.PostAsync(
                "/mobile-phones", new StringContent("{}", Encoding.UTF8, "text/plain"))),
            "json" => ("/mobile-phones", await client.PostAsync(
                "/mobile-phones", new StringContent("{INTERNAL_FAILURE_MARKER", Encoding.UTF8, "application/json"))),
            "missing" => ("/mobile-phones", await client.PostAsync(
                "/mobile-phones", new StringContent("{}", Encoding.UTF8, "application/json"))),
            "body" => ("/mobile-phones", await client.PostAsync(
                "/mobile-phones", new StringContent("", Encoding.UTF8, "application/json"))),
            "binding" => ("/mobile-phones?amount=not-a-number", await client.GetAsync(
                "/mobile-phones?amount=not-a-number")),
            _ => throw new ArgumentOutOfRangeException(nameof(errorCase))
        };
    }

    [Then("the Products REF-06 response has status {int} and code {string}")]
    public async Task ThenTheResponseHasProblemContract(int status, string code)
    {
        _response.ShouldNotBeNull();
        _response.StatusCode.ShouldBe((HttpStatusCode)status);
        _response.Content.Headers.ContentType?.MediaType.ShouldBe("application/problem+json");

        var body = await _response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var problem = document.RootElement;
        problem.GetProperty("status").GetInt32().ShouldBe(status);
        problem.GetProperty("code").GetString().ShouldBe(code);
        problem.GetProperty("type").GetString().ShouldNotBeNullOrWhiteSpace();
        problem.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace();
        problem.GetProperty("detail").GetString().ShouldNotBeNullOrWhiteSpace();
        problem.GetProperty("instance").GetString().ShouldBe(_path!.Split('?')[0]);
        problem.GetProperty("traceId").GetString().ShouldNotBeNullOrWhiteSpace();
        problem.GetProperty("errors").GetArrayLength().ShouldBe(0);
        var missing = problem.GetProperty("missingProperties");
        if (_errorCase == "missing")
        {
            missing.EnumerateArray().Select(item => item.GetString()).ShouldContain("commonDescription");
        }
        else
        {
            missing.GetArrayLength().ShouldBe(0);
        }
        body.ShouldNotContain("INTERNAL_FAILURE_MARKER");
        body.ShouldNotContain("stackTrace");
    }
}
