using System.Net.Mime;
using System.Text;
using System.Text.Json;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Common.Dtos.Currencies;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.Products.Common;

[Binding]
public class SharedProductSteps
{
    private readonly ProductScenarioContext _context;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
    private readonly ScenarioContext _scenarioContext;

    public SharedProductSteps(ProductScenarioContext context, ScenarioContext scenarioContext)
    {
        _context = context;
        _scenarioContext = scenarioContext;
    }

    [Given(@"an existing currency with code ""(.*)""")]
    public async Task GivenAnExistingCurrencyWithCode(string code)
    {
        var payload = new CreateCurrencyExternalDto(code, $"{code} description");
        var content = new StringContent(
            JsonSerializer.Serialize(payload, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

        var response = await TestRunHooks.Client.PostAsync("/currencies", content);

        if (response.StatusCode != System.Net.HttpStatusCode.Created)
        {
            var existing = await TestRunHooks.Client.GetAsync("/currencies");
            existing.EnsureSuccessStatusCode();

            var currencies = await DeserializeResponse<IReadOnlyList<CurrencyDto>>(existing);
            _context.CurrencyCode = currencies?.FirstOrDefault(c => c.Code == code)?.Code ?? string.Empty;
            _context.CurrencyCode.ShouldNotBeNullOrWhiteSpace();
            return;
        }

        var currency = await DeserializeResponse<CurrencyDto>(response);
        currency.ShouldNotBeNull();
        _context.CurrencyCode = currency.Code;
    }

    [When(@"I submit the create product request")]
    public async Task WhenISubmitTheCreateProductRequest()
    {
        _context.Request.ShouldNotBeNull();

        var content = new StringContent(
            JsonSerializer.Serialize(_context.Request, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

        _context.Response = await TestRunHooks.Client.PostAsync("/products", content);
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

    private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }
}
