using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ProductCatalog.Acceptance.Tests.Configuration;

public sealed class OpenApiContractTests
{
    [Theory]
    [InlineData("MobilePhoneDetailsDto", "id", "remove")]
    [InlineData("CommonDescriptionDto", "name", "null")]
    [InlineData("MobilePhoneDetailsDto", "price", "null")]
    [InlineData("MoneyDto", "amount", "\"999.99\"")]
    [InlineData("ConnectivityDto", "has5G", "1")]
    [InlineData("ElectronicDetailsDto", "refreshRateHz", "120.5")]
    [InlineData("ElectronicDetailsDto", "refreshRateHz", "2147483648")]
    [InlineData("CommonDescriptionDto", "otherPhotos", "[1]")]
    [InlineData("MobilePhoneDetailsDto", "id", "\"not-a-uuid\"")]
    [InlineData("MobilePhoneDetailsDto", "unexpected", "true")]
    [InlineData("MobilePhoneHistoryDto", "operation", "999")]
    [InlineData("MobilePhoneHistoryDto", "operation", "\"Inserted\"")]
    [InlineData("MobilePhoneHistoryDto", "changedAt", "\"2026-02-30T12:00:00\"")]
    [InlineData("MobilePhoneHistoryDto", "changedAt", "\"2026-01-02T12:34:56Z\"")]
    [InlineData("ApiProblemDetails", "status", "\"400\"")]
    [InlineData("ApiProblemDetails", "traceId", "null")]
    [InlineData("CreateMobilePhoneExternalDto", "fingerPrint", "remove")]
    [InlineData("CreateMobilePhoneExternalDto", "fingerPrint", "null")]
    public async Task GeneratedSchemasRejectMutatedPayloads(string schemaName, string field, string replacement)
    {
        using var document = await Contract();
        var schemas = Schemas(document);
        var schema = schemas.GetProperty(schemaName);
        var sample = (JsonObject)Sample(schema, schemas)!;
        Assert.Empty(Errors(schema, sample, schemas));
        if (replacement == "remove")
            Assert.True(sample.Remove(field));
        else
            sample[field] = JsonNode.Parse(replacement);
        Assert.NotEmpty(Errors(schema, sample, schemas));
    }

    [Theory]
    [InlineData("{}", true)]
    [InlineData("{\"brand\":null,\"minimalPrice\":null,\"maximalPrice\":null}", true)]
    [InlineData("{\"brand\":\"Samsung\"}", true)]
    [InlineData("{\"brand\":\"sAmSuNg\"}", true)]
    [InlineData("{\"brand\":2,\"minimalPrice\":6.40}", true)]
    [InlineData("{\"brand\":999}", false)]
    [InlineData("{\"brand\":\"Unknown\"}", false)]
    [InlineData("{\"minimalPrice\":\"100\"}", false)]
    public async Task OptionalFilterSchemaDistinguishesOmissionNullNamesAndNumericEnums(string json, bool valid)
    {
        using var document = await Contract();
        using var value = JsonDocument.Parse(json);
        var schemas = Schemas(document);
        Assert.Equal(valid, OpenApiSchemaAssertions.Errors(schemas.GetProperty("MobilePhoneFilterDto"), value.RootElement, schemas).Count == 0);
    }

    [Fact]
    public async Task EveryGeneratedFieldHasSemanticsAndEveryLeafExampleMatchesItsSchema()
    {
        using var document = await Contract();
        var schemas = Schemas(document);
        foreach (var definition in schemas.EnumerateObject())
        {
            var sample = Sample(definition.Value, schemas);
            Assert.Empty(Errors(definition.Value, sample, schemas));
            if (!definition.Value.TryGetProperty("properties", out var properties))
                continue;
            foreach (var property in properties.EnumerateObject())
            {
                Assert.False(string.IsNullOrWhiteSpace(property.Value.GetProperty("description").GetString()), $"{definition.Name}.{property.Name}");
                if (property.Value.TryGetProperty("type", out var type) && type.GetString() is "string" or "integer" or "number" or "boolean")
                    Assert.True(property.Value.TryGetProperty("example", out _), $"Missing leaf example: {definition.Name}.{property.Name}");
            }
        }
        var create = document.RootElement.GetProperty("paths").GetProperty("/mobile-phones").GetProperty("post");
        var location = create.GetProperty("responses").GetProperty("201").GetProperty("headers").GetProperty("Location");
        Assert.True(location.GetProperty("required").GetBoolean());
        Assert.Equal("uri-reference", location.GetProperty("schema").GetProperty("format").GetString());
    }

    [Fact]
    public async Task RequestRequirednessAndUnknownMembersFollowSerializerMetadata()
    {
        using var document = await Contract();
        var schemas = Schemas(document);
        var schema = schemas.GetProperty("CreateMobilePhoneExternalDto");
        var sample = (JsonObject)Sample(schema, schemas)!;
        sample["consumerExtension"] = "ignored by System.Text.Json";
        Assert.Empty(Errors(schema, sample, schemas));
        // A nested record parameter is not automatically JsonRequired. Do not strengthen it as incidental documentation work.
        var money = (JsonObject)sample["price"]!;
        Assert.True(money.Remove("amount"));
        Assert.Empty(Errors(schema, sample, schemas));
        var deserialized = JsonSerializer.Deserialize<CreateMobilePhoneExternalDto>(sample, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        Assert.Equal(0, deserialized!.Price.Amount);
        Assert.True(sample.Remove("price"));
        Assert.NotEmpty(Errors(schema, sample, schemas));
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<CreateMobilePhoneExternalDto>(sample, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
    }

    [Theory]
    [InlineData("/mobile-phones", "{not-json")]
    [InlineData("/mobile-phones/filter", "{\"Brand\":999}")]
    public async Task InvalidRequestsReachTheServerAndTheirProblemsAndEvidenceAreValidated(string path, string raw)
    {
        using var document = await Contract();
        var evidence = new ScenarioHttpEvidence("REF07_CreateMobilePhone_type", "Contract checks", "Invalid JSON", new());
        var received = false;
        using var client = new HttpClient(new OpenApiResponseHandler(document, evidence)
        {
            InnerHandler = new HttpEvidenceHandler(evidence)
            {
                InnerHandler = new Reply(async request =>
                {
                    Assert.Equal(raw, await request.Content!.ReadAsStringAsync());
                    received = true;
                    return Json(HttpStatusCode.BadRequest, Sample(Schemas(document).GetProperty("ApiProblemDetails"), Schemas(document))!, "application/problem+json");
                })
            }
        });
        using var response = await client.PostAsync($"http://localhost{path}", new StringContent(raw, Encoding.UTF8, "application/json"));
        Assert.True(received);
        var records = new List<(string Name, JsonElement Value)>();
        evidence.Flush((name, value) => records.Add((name, JsonSerializer.SerializeToElement(value, ScenarioHttpEvidence.JsonOptions))));
        var requestEvidence = records.Single(item => item.Name == "HTTP 001 request").Value;
        Assert.Equal(raw, requestEvidence.GetProperty("content").GetProperty("rawBody").GetString());
        var observation = records.Single(item => item.Name == "HTTP contract observation").Value;
        Assert.Equal(requestEvidence.GetProperty("exchangeId").GetString(), observation.GetProperty("exchangeId").GetString());
        Assert.True(observation.GetProperty("request").GetProperty("rejectedInvalidInput").GetBoolean());
        Assert.True(observation.GetProperty("response").GetProperty("matchesSchema").GetBoolean());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("/mobile-phones/not-the-created-id")]
    public async Task CreateRejectsMissingOrMismatchedLocation(string? location)
    {
        using var document = await Contract();
        var schemas = Schemas(document);
        using var client = new HttpClient(new OpenApiResponseHandler(document)
        {
            InnerHandler = new Reply(_ =>
            {
                var response = Json(HttpStatusCode.Created, Sample(schemas.GetProperty("MobilePhoneDetailsDto"), schemas)!);
                if (location is not null)
                    response.Headers.Location = new Uri(location, UriKind.Relative);
                return Task.FromResult(response);
            })
        });
        var request = Sample(schemas.GetProperty("CreateMobilePhoneExternalDto"), schemas)!.ToJsonString();
        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => client.PostAsync("http://localhost/mobile-phones", new StringContent(request, Encoding.UTF8, "application/json")));
        Assert.Contains("Location", error.Message);
    }

    [Theory]
    [InlineData("GET", "/mobile-phones?amount=bad", null)]
    [InlineData("GET", "/mobile-phones", null)]
    [InlineData("POST", "/mobile-phones/by-ids", "[\"not-a-uuid\"]")]
    [InlineData("POST", "/mobile-phones/filter", "{\"brand\":999}")]
    public async Task SuccessfulResponsesCannotHideInvalidRequests(string method, string path, string? body)
    {
        using var document = await Contract();
        using var client = new HttpClient(new OpenApiResponseHandler(document)
        {
            InnerHandler = new Reply(_ => Task.FromResult(Json(HttpStatusCode.OK, new JsonArray())))
        });
        using var request = new HttpRequestMessage(new HttpMethod(method), $"http://localhost{path}");
        if (body is not null)
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => client.SendAsync(request));
        Assert.Contains("request.", error.Message);
    }

    [Fact]
    public async Task RepositoryFailureResponsesStillRequireTheCompleteProblemSchema()
    {
        using var document = await Contract();
        var schemas = Schemas(document);
        var problem = (JsonObject)Sample(schemas.GetProperty("ApiProblemDetails"), schemas)!;
        problem["status"] = 500;
        problem["traceId"] = null;
        using var client = new HttpClient(new OpenApiResponseHandler(document)
        {
            InnerHandler = new Reply(_ => Task.FromResult(Json(HttpStatusCode.InternalServerError, problem, "application/problem+json")))
        });
        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetAsync("http://localhost/mobile-phones/top"));
        Assert.Contains("traceId", error.Message);
    }

    [Theory]
    [InlineData("date-time", "\"2026-01-02T12:34:56Z\"", true)]
    [InlineData("date-time", "\"2026-01-02T12:34:56\"", false)]
    [InlineData("uuid", "\"not-a-uuid\"", false)]
    public void StandardFormatsAreCheckedSeparatelyFromTheSqlTimestampFormat(string format, string json, bool valid)
    {
        using var schema = JsonDocument.Parse(JsonSerializer.Serialize(new { type = "string", format }));
        using var value = JsonDocument.Parse(json);
        using var components = JsonDocument.Parse("{}");
        Assert.Equal(valid, OpenApiSchemaAssertions.Errors(schema.RootElement, value.RootElement, components.RootElement).Count == 0);
    }

    private static JsonElement Schemas(JsonDocument document) => document.RootElement.GetProperty("components").GetProperty("schemas");
    private static IReadOnlyList<string> Errors(JsonElement schema, JsonNode? value, JsonElement schemas)
        => OpenApiSchemaAssertions.Errors(schema, JsonSerializer.SerializeToElement(value), schemas);

    private static JsonNode? Sample(JsonElement schema, JsonElement schemas)
    {
        if (schema.TryGetProperty("example", out var example))
            return JsonNode.Parse(example.GetRawText());
        if (schema.TryGetProperty("$ref", out var reference))
            return Sample(schemas.GetProperty(reference.GetString()!.Split('/').Last()), schemas);
        if (schema.TryGetProperty("allOf", out var allOf))
            return Sample(allOf[0], schemas);
        if (schema.TryGetProperty("enum", out var choices))
            return JsonNode.Parse(choices[0].GetRawText());
        if (schema.TryGetProperty("type", out var type))
        {
            if (type.GetString() == "object")
            {
                var result = new JsonObject();
                foreach (var property in schema.GetProperty("properties").EnumerateObject())
                    result[property.Name] = Sample(property.Value, schemas);
                return result;
            }
            if (type.GetString() == "array")
                return new JsonArray();
        }
        throw new InvalidOperationException("Generated contract lacks an example for a leaf schema");
    }

    private static HttpResponseMessage Json(HttpStatusCode status, JsonNode body, string media = "application/json")
        => new(status) { Content = new StringContent(body.ToJsonString(), Encoding.UTF8, media) };

    private static async Task<JsonDocument> Contract()
    {
        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ProductCatalogDb"] = "Server=127.0.0.1,1;Database=ProductsDb;User Id=sa;Password=NotUsed123!;TrustServerCertificate=True;Connect Timeout=1",
                ["Database:ApplyMigrations"] = "false"
            })));
        using var client = factory.CreateClient();
        return JsonDocument.Parse(await client.GetStringAsync("/swagger/v1/swagger.json"));
    }

    private sealed class Reply(Func<HttpRequestMessage, Task<HttpResponseMessage>> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => respond(request);
    }
}
