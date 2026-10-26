using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

/// <summary>Checks actual HTTP exchanges against generated OpenAPI, after evidence has been captured.</summary>
internal sealed class OpenApiResponseHandler(JsonDocument document, ScenarioHttpEvidence? evidence = null) : DelegatingHandler
{
    private JsonElement Components => document.RootElement.GetProperty("components").GetProperty("schemas");

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        var path = request.RequestUri!.AbsolutePath;
        var status = (int)response.StatusCode;
        var requestErrors = new List<string>();
        var responseErrors = new List<string>();
        string? operationId = null;
        var scope = "openapi";

        if (path is "/health/live" or "/health/ready")
        {
            scope = "technical-health-text";
            var text = await response.Content.ReadAsStringAsync(cancellationToken);
            if (response.Content.Headers.ContentType?.MediaType != "text/plain" ||
                status is not (200 or 503) || text != (status == 200 ? "Healthy" : "Unhealthy") ||
                path == "/health/live" && status != 200)
                responseErrors.Add("Health response differs from the explicit text probe contract");
        }
        else
        {
            var route = document.RootElement.GetProperty("paths").EnumerateObject()
                .Where(item => Matches(item.Name, path))
                .OrderByDescending(item => item.Name.Split('/').Count(part => !part.StartsWith('{')))
                .FirstOrDefault(item => item.Value.TryGetProperty(request.Method.Method.ToLowerInvariant(), out _));
            if (route.Value.ValueKind == JsonValueKind.Undefined)
            {
                scope = "framework-routing";
                if (status is not (404 or 405))
                    responseErrors.Add("HTTP operation absent from OpenAPI");
                await CheckProblem(response, responseErrors, cancellationToken);
            }
            else
            {
                var operation = route.Value.GetProperty(request.Method.Method.ToLowerInvariant());
                operationId = operation.GetProperty("operationId").GetString();
                await CheckRequest(request, route.Name, operation, requestErrors, cancellationToken);
                var responses = operation.GetProperty("responses");
                if (responses.TryGetProperty(status.ToString(CultureInfo.InvariantCulture), out var declared))
                    await CheckResponse(response, declared, responseErrors, cancellationToken);
                else if (status == 415)
                {
                    scope = "framework-media";
                    await CheckProblem(response, responseErrors, cancellationToken);
                }
                else
                    responseErrors.Add($"Undeclared runtime status: {operationId} {status}");
            }
        }

        // Rejection scenarios must reach the server. Schema-invalid input is evidence, not a reason to suppress the call.
        // A successful call or a server/repository failure must have a structurally valid request.
        var allowedInvalidRequest = requestErrors.Count > 0 && status is 400 or 415;
        request.Options.TryGetValue(HttpEvidenceHandler.ExchangeIdKey, out var exchangeId);
        evidence?.Record("HTTP contract observation", new
        {
            service = "products",
            scenarioId = evidence.ScenarioId,
            instanceId = evidence.InstanceId,
            exchangeId,
            operationId,
            method = request.Method.Method,
            path,
            status,
            scope,
            request = new
            {
                schemaApplicable = operationId is not null,
                jsonPropertyNameComparison = operationId is null ? null : "ordinal-ignore-case",
                matchesSchema = operationId is null ? (bool?)null : requestErrors.Count == 0,
                rejectedInvalidInput = allowedInvalidRequest,
                errors = requestErrors
            },
            response = new
            {
                kind = scope == "technical-health-text" ? "text-probe" : "json-schema",
                matchesContract = responseErrors.Count == 0,
                matchesSchema = scope == "technical-health-text" ? (bool?)null : responseErrors.Count == 0,
                errors = responseErrors
            }
        });
        if (responseErrors.Count > 0 || requestErrors.Count > 0 && !allowedInvalidRequest)
        {
            response.Dispose();
            throw new InvalidOperationException(string.Join("; ", responseErrors.Concat(allowedInvalidRequest ? [] : requestErrors)));
        }
        return response;
    }

    private async Task CheckRequest(HttpRequestMessage request, string template, JsonElement operation, List<string> errors, CancellationToken cancellation)
    {
        var actual = request.RequestUri!.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var expected = template.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var query = QueryHelpers.ParseQuery(request.RequestUri.Query);
        if (operation.TryGetProperty("parameters", out var parameters))
            foreach (var parameter in parameters.EnumerateArray())
            {
                var name = parameter.GetProperty("name").GetString()!;
                string? raw = null;
                if (parameter.GetProperty("in").GetString() == "path")
                {
                    var index = Array.FindIndex(expected, part => part == $"{{{name}}}");
                    if (index >= 0)
                        raw = Uri.UnescapeDataString(actual[index]);
                }
                else if (query.TryGetValue(name, out var values))
                    raw = values.LastOrDefault();
                if (raw is null)
                {
                    if (parameter.TryGetProperty("required", out var required) && required.GetBoolean())
                        errors.Add($"request.{name}: required parameter missing");
                    continue;
                }
                var schema = parameter.GetProperty("schema");
                using var value = ParameterValue(schema, raw);
                errors.AddRange(OpenApiSchemaAssertions.Errors(schema, value.RootElement, Components, $"request.{name}"));
            }

        if (!operation.TryGetProperty("requestBody", out var body))
            return;
        var bytes = request.Content is null ? [] : await request.Content.ReadAsByteArrayAsync(cancellation);
        if (bytes.Length == 0)
        {
            if (body.TryGetProperty("required", out var required) && required.GetBoolean())
                errors.Add("request.body: required body missing");
            return;
        }
        var media = request.Content!.Headers.ContentType?.MediaType;
        if (media is null || !body.GetProperty("content").TryGetProperty(media, out var representation))
        {
            errors.Add("request.body: unsupported media type");
            return;
        }
        CheckJson(representation.GetProperty("schema"), bytes, "request.body", errors, caseInsensitiveProperties: true);
    }

    private async Task CheckResponse(HttpResponseMessage response, JsonElement declared, List<string> errors, CancellationToken cancellation)
    {
        var bytes = await response.Content.ReadAsByteArrayAsync(cancellation);
        if ((int)response.StatusCode == 204)
        {
            if (bytes.Length != 0 || declared.TryGetProperty("content", out _))
                errors.Add("204 response contains or declares a body");
            return;
        }
        var media = response.Content.Headers.ContentType?.MediaType;
        if (media is null || !declared.TryGetProperty("content", out var content) ||
            !content.TryGetProperty(media, out var representation) || !representation.TryGetProperty("schema", out var schema) || bytes.Length == 0)
        {
            errors.Add("Response media type/body differs from OpenAPI");
            return;
        }
        if (media != ((int)response.StatusCode >= 400 ? "application/problem+json" : "application/json"))
            errors.Add("Unexpected response media type");
        CheckJson(schema, bytes, "response.body", errors);
        if ((int)response.StatusCode >= 400)
            CheckProblemStatus(bytes, (int)response.StatusCode, errors);

        if (declared.TryGetProperty("headers", out var headers))
            foreach (var header in headers.EnumerateObject())
            {
                if (!response.Headers.TryGetValues(header.Name, out var values))
                {
                    if (header.Value.TryGetProperty("required", out var required) && required.GetBoolean())
                        errors.Add($"response.header.{header.Name}: required header missing");
                    continue;
                }
                using var value = JsonDocument.Parse(JsonSerializer.Serialize(values.Single()));
                errors.AddRange(OpenApiSchemaAssertions.Errors(header.Value.GetProperty("schema"), value.RootElement, Components, $"response.header.{header.Name}"));
            }
        if ((int)response.StatusCode == 201 && response.Headers.Location is { } location)
        {
            using var body = JsonDocument.Parse(bytes);
            if (body.RootElement.ValueKind != JsonValueKind.Object || !body.RootElement.TryGetProperty("id", out var id) ||
                id.ValueKind != JsonValueKind.String || location.ToString() != $"/mobile-phones/{id.GetString()}")
                errors.Add("Location does not identify the created phone");
        }
    }

    private async Task CheckProblem(HttpResponseMessage response, List<string> errors, CancellationToken cancellation)
    {
        if (response.Content.Headers.ContentType?.MediaType != "application/problem+json")
            errors.Add("Framework error has unexpected media type");
        var bytes = await response.Content.ReadAsByteArrayAsync(cancellation);
        CheckJson(Components.GetProperty("ApiProblemDetails"), bytes, "response.body", errors);
        CheckProblemStatus(bytes, (int)response.StatusCode, errors);
    }

    private static void CheckProblemStatus(byte[] bytes, int expected, List<string> errors)
    {
        try
        {
            using var body = JsonDocument.Parse(bytes);
            if (body.RootElement.ValueKind != JsonValueKind.Object || !body.RootElement.TryGetProperty("status", out var status) ||
                status.ValueKind != JsonValueKind.Number || !status.TryGetInt32(out var actual) || actual != expected)
                errors.Add("Problem status differs from HTTP status");
        }
        catch (JsonException) { errors.Add("Problem body is not valid JSON"); }
    }

    private void CheckJson(JsonElement schema, byte[] bytes, string path, List<string> errors, bool caseInsensitiveProperties = false)
    {
        try
        {
            using var body = JsonDocument.Parse(bytes);
            errors.AddRange(OpenApiSchemaAssertions.Errors(schema, body.RootElement, Components, path, caseInsensitiveProperties));
        }
        catch (JsonException) { errors.Add($"{path}: malformed JSON"); }
    }

    private static JsonDocument ParameterValue(JsonElement schema, string raw)
    {
        if (schema.TryGetProperty("type", out var type) && type.GetString() is "integer" or "number" or "boolean")
        {
            try { return JsonDocument.Parse(raw); }
            catch (JsonException) { /* Retain a string so scalar validation reports the invalid parameter. */ }
        }
        return JsonDocument.Parse(JsonSerializer.Serialize(raw));
    }

    private static bool Matches(string template, string path)
    {
        var expected = template.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var actual = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return expected.Length == actual.Length && expected.Zip(actual).All(parts =>
            parts.First.StartsWith('{') && parts.First.EndsWith('}') ||
            string.Equals(parts.First, parts.Second, StringComparison.OrdinalIgnoreCase));
    }
}
