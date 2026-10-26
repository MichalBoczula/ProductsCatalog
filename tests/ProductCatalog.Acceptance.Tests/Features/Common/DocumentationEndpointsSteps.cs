using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCatalog.Api;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

[Binding]
public sealed class DocumentationEndpointsSteps(ScenarioApiContext context) : IDisposable
{
    private const string SensitiveDetail = "REF07_INTERNAL_DOCUMENTATION_FAILURE_DO_NOT_EXPOSE";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;
    private HttpResponseMessage? _response;
    private string? _operation;
    private int _calls;

    [Given("the {string} documentation endpoint is selected")]
    public void GivenSelected(string operationId)
    {
        context.Client.ShouldNotBeNull();
        _operation = operationId;
    }

    [Given("the {string} documentation descriptor fails")]
    public void GivenDescriptorFails(string operationId)
    {
        _operation = operationId;
        _factory = context.Factory!.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            switch (operationId)
            {
                case "DescribeAllFlows":
                    services.RemoveAll<IFlowDescriber<CreateMobilePhoneCommand>>();
                    services.AddScoped<IFlowDescriber<CreateMobilePhoneCommand>>(_ => new FailingFlow(() => _calls++));
                    break;
                case "DescribeValidationPolicies":
                    services.RemoveAll<IValidationPolicyDescriptorProvider>();
                    services.AddScoped<IValidationPolicyDescriptorProvider>(_ => new FailingValidation(() => _calls++));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operationId));
            }
        }));
        _client = context.CreateClient(_factory);
    }

    [When("I request the {string} documentation endpoint")]
    public async Task WhenRequest(string operationId)
    {
        operationId.ShouldBe(_operation);
        var path = operationId switch
        {
            "DescribeAllFlows" => "/products-documentation/flow",
            "DescribeValidationPolicies" => "/products-documentation/validation-policies",
            _ => throw new ArgumentOutOfRangeException(nameof(operationId))
        };
        _response = await (_client ?? context.Client!).GetAsync(path);
    }

    [Then("the documentation response has this metadata")]
    public void ThenMetadata(Table table)
    {
        var expected = Values(table);
        _response.ShouldNotBeNull();
        _response.StatusCode.ShouldBe((HttpStatusCode)int.Parse(expected["StatusCode"], CultureInfo.InvariantCulture));
        _response.Content.Headers.ContentType?.MediaType.ShouldBe(expected["ContentType"]);
    }

    [Then("the {string} documentation contains exactly its registered descriptors")]
    public async Task ThenInventory(string operationId, Table table)
    {
        operationId.ShouldBe(_operation);
        _response.ShouldNotBeNull();
        var expected = table.Rows.Where(row => row["Operation"] == operationId).Select(row => row["Descriptor"]).ToArray();
        expected.ShouldNotBeEmpty();
        using var json = JsonDocument.Parse(await _response.Content.ReadAsStringAsync());
        json.RootElement.ValueKind.ShouldBe(JsonValueKind.Array);
        var field = operationId == "DescribeAllFlows" ? "actionName" : "policyName";
        var childField = operationId == "DescribeAllFlows" ? "steps" : "rules";
        var actual = json.RootElement.EnumerateArray().Select(item => item.GetProperty(field).GetString()).ToArray();
        AllureJson.AttachObject("Documentation inventory observation", new
        {
            operationId,
            descriptors = json.RootElement.EnumerateArray().Select(item => new
            {
                name = item.GetProperty(field).GetString(),
                childCount = item.GetProperty(childField).GetArrayLength()
            }).ToArray()
        }, JsonOptions);
        actual.Length.ShouldBe(expected.Length);
        actual.Distinct().Count().ShouldBe(expected.Length);
        actual.OrderBy(name => name, StringComparer.Ordinal).ShouldBe(expected.OrderBy(name => name, StringComparer.Ordinal));
        foreach (var item in json.RootElement.EnumerateArray())
        {
            var children = item.GetProperty(childField);
            children.GetArrayLength().ShouldBeGreaterThan(0);
            if (operationId == "DescribeAllFlows")
            {
                foreach (var step in children.EnumerateArray())
                    step.GetString().ShouldNotBeNullOrWhiteSpace();
            }
            else
            {
                foreach (var rule in children.EnumerateArray())
                {
                    rule.GetProperty("ruleName").GetString().ShouldNotBeNullOrWhiteSpace();
                    var errors = rule.GetProperty("rules");
                    errors.GetArrayLength().ShouldBeGreaterThan(0);
                    foreach (var error in errors.EnumerateArray())
                    {
                        error.GetProperty("message").GetString().ShouldNotBeNullOrWhiteSpace();
                        error.GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace();
                        error.GetProperty("entity").GetString().ShouldNotBeNullOrWhiteSpace();
                    }
                }
            }
        }
    }

    [Then("the documentation error matches this safe problem")]
    public async Task ThenSafeFailure(Table table)
    {
        var expected = Values(table);
        _response.ShouldNotBeNull();
        AllureJson.AttachObject("Documentation failure observation", new { operationId = _operation, descriptorCalls = _calls }, JsonOptions);
        _calls.ShouldBe(int.Parse(expected["DescriptorCalls"], CultureInfo.InvariantCulture));
        var problem = await ApiProblemAssertions.Assert(_response, table);
        problem.GetProperty("title").GetString().ShouldBe(expected["Title"]);
        problem.GetProperty("detail").GetString().ShouldBe(expected["Detail"]);
        var body = await _response.Content.ReadAsStringAsync();
        body.ShouldNotContain(SensitiveDetail);
        body.ShouldNotContain(nameof(InvalidOperationException));
        body.ShouldNotContain(nameof(FailingFlow));
        body.ShouldNotContain(nameof(FailingValidation));
        problem.TryGetProperty("exception", out _).ShouldBeFalse();
    }

    private static Dictionary<string, string> Values(Table table)
        => table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);

    public void Dispose()
    {
        _response?.Dispose();
        _client?.Dispose();
        _factory?.Dispose();
    }

    private sealed class FailingFlow(Action onCall) : IFlowDescriber<CreateMobilePhoneCommand>
    {
        public FlowDescription DescribeFlow(CreateMobilePhoneCommand action)
        {
            onCall();
            throw new InvalidOperationException(SensitiveDetail);
        }
    }

    private sealed class FailingValidation(Action onCall) : IValidationPolicyDescriptorProvider
    {
        public ValidationPolicyDescriptor Describe()
        {
            onCall();
            throw new InvalidOperationException(SensitiveDetail);
        }
    }
}
