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
using System.Net;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

[Binding]
public sealed class DocumentationEndpointsSteps(ScenarioApiContext context) : IDisposable
{
    private const string SensitiveDetail = "REF07_INTERNAL_DOCUMENTATION_FAILURE_DO_NOT_EXPOSE";
    private static readonly string[] FlowNames =
    [
        "CreateMobilePhoneCommand", "UpdateMobilePhoneCommand", "DeleteMobilePhoneCommand", "GetMobilePhoneByIdQuery",
        "GetMobilePhoneByIdsQuery", "GetMobilePhoneHistoryQuery", "GetMobilePhonesQuery", "GetFilteredMobilePhonesQuery",
        "GetTopMobilePhonesQuery"
    ];
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;
    private HttpResponseMessage? _response;
    private string? _path;
    private int _calls;

    [Given("the {string} documentation descriptor fails")]
    public void GivenDescriptorFails(string operationId)
    {
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
        _client = _factory.CreateClient();
    }

    [When("I request the {string} documentation endpoint")]
    public async Task WhenIRequestDocumentation(string operationId)
    {
        _path = operationId switch
        {
            "DescribeAllFlows" => "/products-documentation/flow",
            "DescribeValidationPolicies" => "/products-documentation/validation-policies",
            _ => throw new ArgumentOutOfRangeException(nameof(operationId))
        };
        _response = await (_client ?? context.Client!).GetAsync(_path);
    }

    [Then("the {string} documentation lists registered descriptors")]
    public async Task ThenDocumentationListsDescriptors(string operationId)
    {
        _response.ShouldNotBeNull();
        _response.StatusCode.ShouldBe(HttpStatusCode.OK);
        _response.Content.Headers.ContentType?.MediaType.ShouldBe("application/json");
        using var json = JsonDocument.Parse(await _response.Content.ReadAsStringAsync());
        json.RootElement.ValueKind.ShouldBe(JsonValueKind.Array);
        if (operationId == "DescribeAllFlows")
        {
            json.RootElement.GetArrayLength().ShouldBe(FlowNames.Length);
            var actual = json.RootElement.EnumerateArray()
                .Select(item => item.GetProperty("actionName").GetString()).ToArray();
            actual.Distinct().Count().ShouldBe(FlowNames.Length);
            foreach (var name in FlowNames)
            {
                actual.ShouldContain(name);
            }
            foreach (var item in json.RootElement.EnumerateArray())
            {
                item.GetProperty("steps").GetArrayLength().ShouldBeGreaterThan(0);
            }
        }
        else
        {
            operationId.ShouldBe("DescribeValidationPolicies");
            json.RootElement.GetArrayLength().ShouldBe(5);
            foreach (var item in json.RootElement.EnumerateArray())
            {
                item.GetProperty("policyName").GetString().ShouldNotBeNullOrWhiteSpace();
            }
        }
    }

    [Then("the documentation error is safe and the descriptor was called")]
    public async Task ThenDocumentationFailureIsSafe()
    {
        _response.ShouldNotBeNull();
        _calls.ShouldBe(1);
        _response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        _response.Content.Headers.ContentType?.MediaType.ShouldBe("application/problem+json");
        var body = await _response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(body);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe(500);
        json.RootElement.GetProperty("code").GetString().ShouldBe("internal_error");
        json.RootElement.GetProperty("instance").GetString().ShouldBe(_path);
        json.RootElement.GetProperty("traceId").GetString().ShouldNotBeNullOrWhiteSpace();
        body.ShouldNotContain(SensitiveDetail);
        body.ShouldNotContain("stackTrace");
    }

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
