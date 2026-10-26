using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

internal static class ApiProblemAssertions
{
    internal static async Task<JsonElement> Assert(HttpResponseMessage response, Table table)
    {
        var expected = table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
        var status = int.Parse(expected["StatusCode"], CultureInfo.InvariantCulture);
        response.StatusCode.ShouldBe((HttpStatusCode)status);
        response.Content.Headers.ContentType?.MediaType.ShouldBe(expected["ContentType"]);
        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var problem = document.RootElement;
        problem.GetProperty("status").GetInt32().ShouldBe(status);
        problem.GetProperty("code").GetString().ShouldBe(expected["Code"]);
        problem.GetProperty("type").GetString().ShouldNotBeNullOrWhiteSpace();
        problem.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace();
        problem.GetProperty("detail").GetString().ShouldNotBeNullOrWhiteSpace();
        problem.GetProperty("instance").GetString().ShouldBe(response.RequestMessage!.RequestUri!.AbsolutePath);
        problem.GetProperty("traceId").GetString().ShouldNotBeNullOrWhiteSpace();
        problem.GetProperty("errors").GetArrayLength().ShouldBe(int.Parse(expected["ErrorCount"], CultureInfo.InvariantCulture));
        var missing = problem.GetProperty("missingProperties");
        missing.GetArrayLength().ShouldBe(int.Parse(expected["MissingPropertiesCount"], CultureInfo.InvariantCulture));
        if (expected.TryGetValue("MissingProperties", out var members))
            missing.EnumerateArray().Select(item => item.GetString()).OrderBy(value => value, StringComparer.Ordinal)
                .ShouldBe(members.Split(',', StringSplitOptions.TrimEntries).OrderBy(value => value, StringComparer.Ordinal));
        body.ShouldNotContain("INTERNAL_FAILURE_MARKER");
        body.ShouldNotContain("stackTrace");
        return problem.Clone();
    }
}
