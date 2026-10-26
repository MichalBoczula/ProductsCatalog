using ProductCatalog.Acceptance.Tests.Features.Common;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Configuration;

public sealed class DocumentationEvidenceTests
{
    [Fact]
    public void CatalogueDistinguishesInvokedPoliciesStandaloneRulesAndTheInlineGuard()
    {
        var source = DocumentationEvidence.Catalogue.Read();
        Assert.Equal(49, source.Cases.Count);
        Assert.Equal(9, source.OperationFlows.Count);
        Assert.Contains("products.rule.hardware-numbers", source.RuleNames.Values);
        Assert.DoesNotContain(source.OperationFlows.Values, flow => flow.RuleIds.Any(id => id.StartsWith("products.rule.hardware-", StringComparison.Ordinal)));
        Assert.Contains("products.guard.selected-ids", source.OperationFlows["GetMobilePhonesByIds"].RuleIds);
        Assert.Equal("products.policy.filter", Assert.Single(source.OperationFlows["GetFiltered MobilePhones"].PolicyIds));
    }

    [Theory]
    [InlineData("Given", false)]
    [InlineData("When", true)]
    public async Task SetupWithTheSameOperationAndStatusDoesNotClaimTheRepeatedDeleteBranch(string setupPhase, bool explicitlySupporting)
    {
        var evidence = Evidence("REF08_DeleteMobilePhone_200_Repeated");
        var links = new DocumentationEvidence(evidence.ScenarioId);
        using var response = Reply(200);
        using var setup = Request(setupPhase);
        setup.Options.Set(HttpEvidenceHandler.SupportingExchangeKey, explicitlySupporting);
        await links.Observe(evidence, setup, response, "DeleteMobilePhone", "setup", "openapi", default);
        Assert.Throws<InvalidOperationException>(links.Complete);
        using var action = Request("When");
        await links.Observe(evidence, action, response, "DeleteMobilePhone", "primary", "openapi", default);
        links.Complete();
        var records = Drain(evidence);
        Assert.Null(records[0].GetProperty("expectedBranchId").GetString());
        Assert.Equal("products.branch.delete.repeated", records[1].GetProperty("expectedBranchId").GetString());
        Assert.Equal("primary", records[1].GetProperty("exchangeId").GetString());
    }

    [Fact]
    public async Task APrimaryValidationCaseLinksItsActualErrorRule()
    {
        var evidence = Evidence("REF07_CreateMobilePhone_02");
        var links = new DocumentationEvidence(evidence.ScenarioId);
        using var request = Request("When");
        using var response = Reply(400, "{\"code\":\"validation_failed\",\"errors\":[{\"name\":\"MobilePhonesStringValidationRule\"}]}");
        await links.Observe(evidence, request, response, "CreateMobilePhone", "primary", "openapi", default);
        links.Complete();
        var actual = Assert.Single(Drain(evidence));
        Assert.Equal("products.rule.phone-strings", actual.GetProperty("observedErrorRuleIds")[0].GetString());
        Assert.Equal("products.branch.create.validation", actual.GetProperty("expectedBranchId").GetString());
    }

    [Fact]
    public async Task AValidationStatusAloneCannotSatisfyAnExpectedRule()
    {
        var evidence = Evidence("REF07_CreateMobilePhone_02");
        var links = new DocumentationEvidence(evidence.ScenarioId);
        using var request = Request("When");
        using var response = Reply(400, "{\"code\":\"validation_failed\",\"errors\":[]}");
        await Assert.ThrowsAsync<InvalidOperationException>(() => links.Observe(evidence, request, response, "CreateMobilePhone", "primary", "openapi", default));
        Assert.Empty(Assert.Single(Drain(evidence)).GetProperty("observedErrorRuleIds").EnumerateArray());
    }

    [Fact]
    public async Task AnActualStandaloneRuleCannotBePresentedAsInvokedByThePhoneFlow()
    {
        var evidence = Evidence("REF07_CreateMobilePhone_02");
        var links = new DocumentationEvidence(evidence.ScenarioId);
        using var request = Request("When");
        using var response = Reply(400, "{\"code\":\"validation_failed\",\"errors\":[{\"name\":\"MobilePhonesStringValidationRule\"},{\"name\":\"ElectronicDetailsNumbersValidationRule\"}]}");
        await Assert.ThrowsAsync<InvalidOperationException>(() => links.Observe(evidence, request, response, "CreateMobilePhone", "primary", "openapi", default));
        Assert.False(Assert.Single(Drain(evidence)).GetProperty("observedRulesDeclaredByNominalFlow").GetBoolean());
    }

    [Fact]
    public async Task TechnicalRoutingHasItsOwnExplicitBranchAndNoNominalBusinessFlow()
    {
        var evidence = Evidence("REF06_Framework_RouteNotFound");
        var links = new DocumentationEvidence(evidence.ScenarioId, "products.branch.http.route");
        using var request = Request("When");
        using var response = Reply(404, "{\"code\":\"route_not_found\",\"errors\":[]}");
        await links.Observe(evidence, request, response, null, "primary", "framework-routing", default);
        links.Complete();
        var actual = Assert.Single(Drain(evidence));
        Assert.Null(actual.GetProperty("nominalFlowId").GetString());
        Assert.Equal("framework", actual.GetProperty("expectedBranchStage").GetString());
    }

    [Fact]
    public async Task AHealthProbeDoesNotInventABusinessBranchOrRule()
    {
        var evidence = Evidence("REF09_Health_Recovery");
        var links = new DocumentationEvidence(evidence.ScenarioId);
        using var request = Request("When");
        using var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable) { Content = new StringContent("Unhealthy") };
        await links.Observe(evidence, request, response, null, "probe", "technical-health-text", default);
        links.Complete();
        var actual = Assert.Single(Drain(evidence));
        Assert.Null(actual.GetProperty("expectedBranchId").GetString());
        Assert.Empty(actual.GetProperty("observedErrorRuleIds").EnumerateArray());
    }

    private static ScenarioHttpEvidence Evidence(string id) => new(id, "Documentation links", "Contract", new());
    private static HttpRequestMessage Request(string phase)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/mobile-phones");
        request.Options.Set(HttpEvidenceHandler.StepPhaseKey, phase);
        return request;
    }
    private static HttpResponseMessage Reply(int status, string? problem = null) => new((HttpStatusCode)status)
    {
        Content = new StringContent(problem ?? "{}", Encoding.UTF8, problem is null ? "application/json" : "application/problem+json")
    };
    private static List<JsonElement> Drain(ScenarioHttpEvidence evidence)
    {
        var result = new List<JsonElement>();
        evidence.Flush((_, value) => result.Add(JsonSerializer.SerializeToElement(value, ScenarioHttpEvidence.JsonOptions)));
        return result;
    }
}
