using ProductCatalog.Acceptance.Tests.Features.Common;
using Reqnroll;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Configuration;

public sealed class HttpEvidenceTests
{
    [Theory]
    [InlineData("application/json", "{\"name\":\"żółć\",\"active\":false}", "json")]
    [InlineData("application/json", "{bad json", "invalid-json")]
    [InlineData("application/json", "null", "json")]
    [InlineData("text/plain", "Unhealthy", "text")]
    [InlineData("application/json", "", "empty")]
    public async Task BodiesAreCapturedWithoutChangingWhatTheServerOrCallerReads(string media, string raw, string format)
    {
        var evidence = Evidence();
        using var client = Client(evidence, async (request, cancellation) =>
        {
            Assert.Equal(raw, await request.Content!.ReadAsStringAsync(cancellation));
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(raw, Encoding.UTF8, media) };
            response.Headers.Location = new Uri("/mobile-phones/created", UriKind.Relative);
            return response;
        });
        using var response = await client.PostAsync("http://localhost/mobile-phones?amount=0", new StringContent(raw, Encoding.UTF8, media));
        Assert.Equal(raw, await response.Content.ReadAsStringAsync());
        var items = Drain(evidence);
        Assert.Equal(2, items.Count);
        var request = items[0];
        Assert.Equal("POST", request.GetProperty("method").GetString());
        Assert.Equal("/mobile-phones", request.GetProperty("path").GetString());
        Assert.Equal("?amount=0", request.GetProperty("query").GetString());
        Assert.Equal("REF07_CreateMobilePhone_type", request.GetProperty("scenarioId").GetString());
        Assert.Equal(request.GetProperty("exchangeId").GetString(), items[1].GetProperty("exchangeId").GetString());
        foreach (var item in items)
        {
            var content = item.GetProperty("content");
            Assert.Equal(raw, content.GetProperty("rawBody").GetString());
            Assert.Equal(format, content.GetProperty("bodyFormat").GetString());
            Assert.Equal(Encoding.UTF8.GetByteCount(raw), content.GetProperty("byteLength").GetInt32());
            if (format == "json")
            {
                using var expected = JsonDocument.Parse(raw);
                Assert.True(JsonElement.DeepEquals(expected.RootElement, content.GetProperty("body")));
            }
            else if (format != "empty")
                Assert.Equal(raw, content.GetProperty("body").GetString());
        }
        Assert.Equal(400, items[1].GetProperty("status").GetInt32());
        Assert.Equal("/mobile-phones/created", items[1].GetProperty("location").GetString());
    }

    [Fact]
    public async Task BodylessRequestsHaveNoInventedJsonAndMultipleClientsShareTheSequence()
    {
        var evidence = Evidence();
        using var first = Client(evidence, (_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent)));
        using var second = Client(evidence, (_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent)));
        using var get = await first.GetAsync("http://localhost/health/live");
        using var delete = await second.DeleteAsync("http://localhost/mobile-phones/id");
        var items = Drain(evidence);
        Assert.Equal(4, items.Count);
        Assert.NotEqual(items[0].GetProperty("exchangeId").GetString(), items[2].GetProperty("exchangeId").GetString());
        Assert.Equal("none", items[0].GetProperty("content").GetProperty("bodyFormat").GetString());
        Assert.Equal(JsonValueKind.Null, items[2].GetProperty("content").GetProperty("body").ValueKind);
        Assert.Equal("empty", items[1].GetProperty("content").GetProperty("bodyFormat").GetString());
    }

    [Fact]
    public async Task NestedServerCallsWithoutTheCallersExecutionContextKeepTheirEvidence()
    {
        var evidence = Evidence();
        using var inner = Client(evidence, (_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
        using var outer = Client(evidence, async (_, _) =>
        {
            Task<HttpResponseMessage> pending;
            using (ExecutionContext.SuppressFlow())
                pending = Task.Run(() => inner.GetAsync("http://localhost/mobile-phones/winner"));
            using var nested = await pending;
            return new HttpResponseMessage(HttpStatusCode.Conflict);
        });
        using var response = await outer.DeleteAsync("http://localhost/mobile-phones/loser");
        var items = Drain(evidence);
        Assert.Equal(4, items.Count);
        Assert.Equal(2, items.Select(item => item.GetProperty("exchangeId").GetString()).Distinct().Count());
        Assert.All(items, item => Assert.Equal(evidence.ScenarioId, item.GetProperty("scenarioId").GetString()));
        Assert.Equal(409, items[3].GetProperty("status").GetInt32());
    }

    [Fact]
    public async Task EvidenceSurvivesAnOuterValidationFailure()
    {
        var evidence = Evidence();
        using var client = new HttpClient(new RejectResponse
        {
            InnerHandler = new HttpEvidenceHandler(evidence)
            {
                InnerHandler = new StubHandler((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent("{\"id\":42}", Encoding.UTF8, "application/json") }))
            }
        });
        await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetAsync("http://localhost/mobile-phones"));
        Assert.Equal(200, Drain(evidence)[1].GetProperty("status").GetInt32());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task TransportErrorsAreRecordedAndRethrownWithoutLeakingExceptionMessages(bool cancelled)
    {
        var evidence = Evidence();
        using var client = Client(evidence, (_, _) => throw (cancelled
            ? new OperationCanceledException("sensitive message") : new HttpRequestException("sensitive message")));
        var error = await Record.ExceptionAsync(() => client.GetAsync("http://localhost/mobile-phones"));
        Assert.True(cancelled ? error is OperationCanceledException : error is HttpRequestException);
        var items = Drain(evidence);
        Assert.Equal(2, items.Count);
        Assert.Equal(cancelled, items[1].GetProperty("cancelled").GetBoolean());
        Assert.DoesNotContain("sensitive message", items[1].GetRawText());
        Assert.Empty(Drain(evidence));
    }

    [Fact]
    public void IdentityReusesMatrixIdsAndIncludesExamplesWithoutDependingOnScenarioWording()
    {
        var arguments = new OrderedDictionary { ["scenarioId"] = "REF07_GetMobilePhones_03", ["amount"] = "0" };
        var first = ScenarioHttpEvidence.From(new ScenarioInfo("old title", "", [], arguments), "Phones");
        var renamed = ScenarioHttpEvidence.From(new ScenarioInfo("new title", "", [], arguments), "Phones");
        Assert.Equal("REF07_GetMobilePhones_03", first.ScenarioId);
        Assert.Equal(first.InstanceId, renamed.InstanceId);
        arguments["amount"] = "-1";
        var otherExample = ScenarioHttpEvidence.From(new ScenarioInfo("old title", "", [], arguments), "Phones");
        Assert.NotEqual(first.InstanceId, otherExample.InstanceId);
        Assert.Throws<InvalidOperationException>(() => ScenarioHttpEvidence.From(new ScenarioInfo("missing ID", "", [], new OrderedDictionary()), "Phones"));
        var tagged = ScenarioHttpEvidence.From(new ScenarioInfo("tagged", "", ["allure.label.scenarioId:REF09_Health_Live"], new OrderedDictionary()), "Health");
        Assert.Equal("REF09_Health_Live", tagged.ScenarioId);
    }

    private static ScenarioHttpEvidence Evidence() => new("REF07_CreateMobilePhone_type", "Phones", "Create phone", new());
    private static HttpClient Client(ScenarioHttpEvidence evidence, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send)
        => new(new HttpEvidenceHandler(evidence) { InnerHandler = new StubHandler(send) });
    private static List<JsonElement> Drain(ScenarioHttpEvidence evidence)
    {
        var items = new List<JsonElement>();
        evidence.Flush((_, value) => items.Add(JsonSerializer.SerializeToElement(value, ScenarioHttpEvidence.JsonOptions)));
        return items;
    }

    private sealed class StubHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => send(request, cancellationToken);
    }
    private sealed class RejectResponse : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using var response = await base.SendAsync(request, cancellationToken);
            throw new InvalidOperationException("Contract mismatch");
        }
    }
}
