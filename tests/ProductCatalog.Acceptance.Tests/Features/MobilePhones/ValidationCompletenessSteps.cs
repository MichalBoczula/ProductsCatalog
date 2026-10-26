using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using Reqnroll;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones;

[Binding]
public sealed class ValidationCompletenessSteps(ScenarioApiContext apiContext) : IDisposable
{
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);
    private Guid _phoneId;
    private HttpResponseMessage? _response;

    [Given("a saved baseline phone for the validation audit")]
    public async Task GivenBaselinePhone()
    {
        using var response = await apiContext.Client!.PostAsJsonAsync(
            "/mobile-phones", CreateMobilePhoneSteps.BuildMobilePhoneRequest(null), _json);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var phone = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_json);
        phone.ShouldNotBeNull();
        _phoneId = phone.Id;
    }

    [When("I submit this blank phone field")]
    public async Task WhenBlankField(Table table)
    {
        var values = Values(table);
        var payload = JsonSerializer.SerializeToNode(CreateMobilePhoneSteps.BuildMobilePhoneRequest(null), _json)!;
        var segments = values["Field"].Split('.');
        var owner = segments.Length == 1 ? payload : payload[segments[0]]!;
        owner[segments[^1]] = values["Value"];
        using var request = new HttpRequestMessage(new HttpMethod(values["Method"]),
            values["Method"] == "POST" ? "/mobile-phones" : $"/mobile-phones/{_phoneId}")
        {
            Content = JsonContent.Create(payload, options: _json)
        };
        _response = await apiContext.Client!.SendAsync(request);
    }

    [When("I submit this price filter for the validation audit")]
    public async Task WhenPriceFilter(Table table)
    {
        var payload = JsonNode.Parse(Values(table)["JsonBody"])!;
        _response = await apiContext.Client!.PostAsJsonAsync("/mobile-phones/filter", payload, _json);
    }

    [Then("the audit rejection contains this exact validation error")]
    public async Task ThenValidationError(Table table)
    {
        _response.ShouldNotBeNull();
        var expected = Values(table);
        var problem = await ApiProblemAssertions.Assert(_response, table);
        problem.GetProperty("title").GetString().ShouldBe(expected["Title"]);
        problem.GetProperty("detail").GetString().ShouldBe(expected["Detail"]);
        var error = problem.GetProperty("errors").EnumerateArray().Single();
        error.GetProperty("name").GetString().ShouldBe(expected["ErrorName"]);
        error.GetProperty("entity").GetString().ShouldBe(expected["ErrorEntity"]);
        error.GetProperty("message").GetString().ShouldBe(expected["ErrorMessage"]);
    }

    private static Dictionary<string, string> Values(Table table)
        => table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);

    public void Dispose() => _response?.Dispose();
}
