using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

/// <summary>Validates the OpenAPI 3.0 schema vocabulary emitted by this service, without DTO deserialization.</summary>
internal static class OpenApiSchemaAssertions
{
    internal static IReadOnlyList<string> Errors(JsonElement schema, JsonElement value, JsonElement components, string path = "$", bool caseInsensitiveProperties = false)
    {
        var errors = new List<string>();
        Check(schema, value, components, path, errors, 0, caseInsensitiveProperties);
        return errors;
    }

    private static void Check(JsonElement schema, JsonElement value, JsonElement components, string path, List<string> errors, int depth, bool caseInsensitiveProperties)
    {
        if (depth > 64)
            throw new InvalidOperationException($"Schema validation depth exceeded at {path}");
        if (schema.TryGetProperty("$ref", out var reference))
        {
            const string prefix = "#/components/schemas/";
            var name = reference.GetString()!;
            if (!name.StartsWith(prefix, StringComparison.Ordinal))
                throw new InvalidOperationException($"Unsupported contract reference at {path}");
            Check(components.GetProperty(name[prefix.Length..]), value, components, path, errors, depth + 1, caseInsensitiveProperties);
            return; // OpenAPI 3.0 Reference Object siblings do not override its target.
        }

        if (schema.TryGetProperty("allOf", out var allOf))
            foreach (var part in allOf.EnumerateArray())
                Check(part, value, components, path, errors, depth + 1, caseInsensitiveProperties);
        foreach (var keyword in new[] { "anyOf", "oneOf" })
            if (schema.TryGetProperty(keyword, out var alternatives))
            {
                var matches = alternatives.EnumerateArray().Count(part =>
                {
                    var branch = new List<string>();
                    Check(part, value, components, path, branch, depth + 1, caseInsensitiveProperties);
                    return branch.Count == 0;
                });
                if (matches == 0 || keyword == "oneOf" && matches != 1)
                    errors.Add($"{path}: {keyword} mismatch");
            }

        if (schema.TryGetProperty("enum", out var choices) &&
            !choices.EnumerateArray().Any(choice => JsonElement.DeepEquals(choice, value)))
            errors.Add($"{path}: value outside enum");

        if (value.ValueKind == JsonValueKind.Null)
        {
            if (schema.TryGetProperty("type", out _) &&
                (!schema.TryGetProperty("nullable", out var nullable) || nullable.ValueKind != JsonValueKind.True))
                errors.Add($"{path}: null is not allowed");
            return;
        }
        if (!schema.TryGetProperty("type", out var type))
            return; // A composition or unconstrained additional-properties schema can have no local type.
        var kind = type.GetString();
        var validType = kind switch
        {
            "object" => value.ValueKind == JsonValueKind.Object,
            "array" => value.ValueKind == JsonValueKind.Array,
            "string" => value.ValueKind == JsonValueKind.String,
            "boolean" => value.ValueKind is JsonValueKind.True or JsonValueKind.False,
            "integer" => value.ValueKind == JsonValueKind.Number && value.TryGetDecimal(out var integer) && decimal.Truncate(integer) == integer,
            "number" => value.ValueKind == JsonValueKind.Number && value.TryGetDouble(out var number) && double.IsFinite(number),
            _ => throw new InvalidOperationException($"Unsupported schema type at {path}: {kind}")
        };
        if (!validType)
        {
            errors.Add($"{path}: expected {kind}");
            return;
        }

        if (kind == "object")
        {
            if (schema.TryGetProperty("required", out var required))
                foreach (var field in required.EnumerateArray())
                    if (!TryProperty(value, field.GetString()!, caseInsensitiveProperties, out _))
                        errors.Add($"{path}.{field.GetString()}: required field missing");
            schema.TryGetProperty("properties", out var properties);
            schema.TryGetProperty("additionalProperties", out var additional);
            foreach (var property in value.EnumerateObject())
            {
                if (properties.ValueKind == JsonValueKind.Object && TryProperty(properties, property.Name, caseInsensitiveProperties, out var childSchema))
                    Check(childSchema, property.Value, components, $"{path}.{property.Name}", errors, depth + 1, caseInsensitiveProperties);
                else if (additional.ValueKind == JsonValueKind.False)
                    errors.Add($"{path}.{property.Name}: additional property forbidden");
                else if (additional.ValueKind == JsonValueKind.Object)
                    Check(additional, property.Value, components, $"{path}.{property.Name}", errors, depth + 1, caseInsensitiveProperties);
            }
        }
        if (kind == "array" && schema.TryGetProperty("items", out var items))
        {
            var index = 0;
            foreach (var item in value.EnumerateArray())
                Check(items, item, components, $"{path}[{index++}]", errors, depth + 1, caseInsensitiveProperties);
        }
        if (kind == "string" && schema.TryGetProperty("pattern", out var pattern) &&
            !Regex.IsMatch(value.GetString()!, pattern.GetString()!, RegexOptions.CultureInvariant, TimeSpan.FromSeconds(1)))
            errors.Add($"{path}: pattern mismatch");
        if (schema.TryGetProperty("format", out var format) && !MatchesFormat(format.GetString()!, value))
            errors.Add($"{path}: invalid {format.GetString()} format");
    }

    private static bool TryProperty(JsonElement element, string name, bool ignoreCase, out JsonElement value)
    {
        if (element.TryGetProperty(name, out value))
            return true;
        if (ignoreCase)
            foreach (var property in element.EnumerateObject())
                if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    value = property.Value;
                    return true;
                }
        return false;
    }

    private static bool MatchesFormat(string format, JsonElement value) => format switch
    {
        "int32" => value.TryGetDecimal(out var i32) && i32 >= int.MinValue && i32 <= int.MaxValue,
        "int64" => value.TryGetDecimal(out var i64) && i64 >= long.MinValue && i64 <= long.MaxValue,
        "float" => value.TryGetSingle(out var single) && float.IsFinite(single),
        "double" => value.TryGetDouble(out var number) && double.IsFinite(number),
        "uuid" => Guid.TryParseExact(value.GetString(), "D", out _),
        "uri" => Uri.TryCreate(value.GetString(), UriKind.Absolute, out _),
        "uri-reference" => !value.GetString()!.Any(char.IsWhiteSpace) && Uri.TryCreate(value.GetString(), UriKind.RelativeOrAbsolute, out _),
        "date-time" => Regex.IsMatch(value.GetString()!, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?(Z|[+-]\d{2}:\d{2})$", RegexOptions.CultureInvariant) &&
            DateTimeOffset.TryParse(value.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out _),
        "date-time-local" => Regex.IsMatch(value.GetString()!, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{1,7})?$", RegexOptions.CultureInvariant) &&
            DateTime.TryParse(value.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out _),
        _ => throw new InvalidOperationException($"Unsupported contract format: {format}")
    };
}
