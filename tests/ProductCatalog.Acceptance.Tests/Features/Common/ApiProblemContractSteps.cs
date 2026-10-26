using ProductCatalog.Acceptance.Tests.Features.MobilePhones;
using Reqnroll;
using Shouldly;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

[Binding]
public sealed class ApiProblemContractSteps(ScenarioApiContext apiContext) : IDisposable
{
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);
    private HttpRequestMessage? _request;
    private HttpResponseMessage? _response;
    private string? _contentType;

    [Given("the rejected HTTP request has these details")]
    public void GivenHttpRequest(Table table)
    {
        var values = Values(table);
        _request = new HttpRequestMessage(new HttpMethod(values["Method"]), values["Path"]);
        _contentType = values.GetValueOrDefault("ContentType");
    }

    [Given("the rejected HTTP request has this raw body")]
    public void GivenRawBody(string body) => SetBody(body);

    [Given("the rejected HTTP request has an empty body")]
    public void GivenEmptyBody() => SetBody(string.Empty);

    [Given("an otherwise valid create body has this JSON member replaced")]
    public void GivenReplacedMember(Table table)
    {
        var values = Values(table);
        var body = ValidCreateBody();
        body.ContainsKey(values["Member"]).ShouldBeTrue();
        body[values["Member"]] = JsonNode.Parse(values["JsonValue"]);
        SetBody(body.ToJsonString(_json));
    }

    [Given("an otherwise valid create body omits this JSON member")]
    public void GivenOmittedMember(Table table)
    {
        var body = ValidCreateBody();
        body.Remove(Values(table)["Member"]).ShouldBeTrue();
        SetBody(body.ToJsonString(_json));
    }

    [When("I send the rejected HTTP request")]
    public async Task WhenSendRequest()
    {
        _request.ShouldNotBeNull();
        _response = await apiContext.Client!.SendAsync(_request);
    }

    [Then("the rejected HTTP response has this safe problem")]
    public async Task ThenSafeProblem(Table table)
    {
        _response.ShouldNotBeNull();
        var problem = await ApiProblemAssertions.Assert(_response, table);
        var expected = Values(table);
        problem.GetProperty("title").GetString().ShouldBe(expected["Title"]);
        problem.GetProperty("detail").GetString().ShouldBe(expected["Detail"]);
    }

    private void SetBody(string body)
    {
        _request.ShouldNotBeNull();
        _contentType.ShouldNotBeNullOrWhiteSpace();
        _request.Content = new StringContent(body, Encoding.UTF8, _contentType);
    }

    private JsonObject ValidCreateBody() => JsonSerializer.SerializeToNode(
        CreateMobilePhoneSteps.BuildMobilePhoneRequest(null), _json)!.AsObject();

    private static Dictionary<string, string> Values(Table table)
        => table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);

    public void Dispose()
    {
        _response?.Dispose();
        _request?.Dispose();
    }
}
