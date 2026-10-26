using Microsoft.OpenApi;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Common;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Nodes;

namespace ProductCatalog.Api.Configuration.Common;

/// <summary>HTTP documentation metadata; DTOs and serializer metadata still define the shapes.</summary>
public sealed class ApiContractSchemaFilter : ISchemaFilter
{
    private static readonly HashSet<Type> ResponseTypes =
    [
        typeof(MobilePhoneDetailsDto), typeof(MobilePhoneDto), typeof(MobilePhoneHistoryDto),
        typeof(TopMobilePhoneDto), typeof(TopMobilePhonesCommonDescriptionDto),
        typeof(CommonDescriptionDto), typeof(ElectronicDetailsDto), typeof(MoneyDto),
        typeof(ConnectivityDto), typeof(SatelliteNavigationSystemDto), typeof(SensorsDto),
        typeof(FlowDescription), typeof(ValidationPolicyDescriptor), typeof(ValidationRuleDescriptor),
        typeof(ValidationError), typeof(ApiProblemDetails)
    ];

    private static readonly Dictionary<string, (string Description, string? Example)> Fields = new()
    {
        ["id"] = ("Server-generated identifier; in a history item this identifies the history row, not the phone.", "\"4f769bfe-bb85-4ba3-b705-36042c1049fc\""),
        ["mobilePhoneId"] = ("Identifier of the phone whose snapshot this history row records.", "\"4f769bfe-bb85-4ba3-b705-36042c1049fc\""),
        ["commonDescription"] = ("Name, brand, descriptive text and photo references for the phone.", null),
        ["name"] = ("Display name of the phone; create/update reject blank names.", "\"Galaxy S24\""),
        ["brand"] = ("Free-text phone brand; create/update reject blank brands. The filter uses a separate enum.", "\"Samsung\""),
        ["description"] = ("Main product description; create/update reject blank text.", "\"An Android phone with an OLED display.\""),
        ["mainPhoto"] = ("Main photo reference as stored. May be a relative filename or URL; not constrained to an absolute URI.", "\"main-photo.jpg\""),
        ["otherPhotos"] = ("Additional stored photo references, in supplied order; an empty array is allowed.", "[\"side-photo.jpg\"]"),
        ["electronicDetails"] = ("Hardware specifications. The published ElectronicDetails policy is standalone and is not invoked by create/update.", null),
        ["cpu"] = ("CPU model text; no hardware-unit conversion is performed.", "\"Octa-core\""),
        ["gpu"] = ("GPU model text.", "\"Adreno\""),
        ["ram"] = ("RAM capacity as free text, including its unit.", "\"8GB\""),
        ["storage"] = ("Storage capacity as free text, including its unit.", "\"256GB\""),
        ["displayType"] = ("Display technology text.", "\"OLED\""),
        ["refreshRateHz"] = ("Display refresh rate in hertz; create/update do not invoke the standalone hardware validation policy.", "120"),
        ["screenSizeInches"] = ("Screen diagonal in inches, represented as a JSON number.", "6.4"),
        ["width"] = ("Stored handset width value; the API does not convert units.", "72"),
        ["height"] = ("Stored handset height value; the API does not convert units.", "152"),
        ["batteryType"] = ("Battery technology text.", "\"Li-Ion\""),
        ["batteryCapacity"] = ("Stored battery capacity value; the API does not convert units.", "4500"),
        ["connectivity"] = ("Supported connectivity features; false means the feature is absent.", null),
        ["has5G"] = ("Whether the phone supports 5G connectivity.", "true"),
        ["wiFi"] = ("Whether the phone supports Wi-Fi.", "true"),
        ["nfc"] = ("Whether the phone supports NFC.", "true"),
        ["bluetooth"] = ("Whether the phone supports Bluetooth.", "true"),
        ["satelliteNavigationSystems"] = ("Supported satellite navigation systems.", null),
        ["gps"] = ("Whether GPS is supported.", "true"),
        ["agps"] = ("Whether assisted GPS is supported.", "true"),
        ["galileo"] = ("Whether Galileo is supported.", "true"),
        ["glonass"] = ("Whether GLONASS is supported.", "true"),
        ["qzss"] = ("Whether QZSS is supported.", "true"),
        ["sensors"] = ("Supported sensors; false means the sensor is absent.", null),
        ["accelerometer"] = ("Whether an accelerometer is present.", "true"),
        ["gyroscope"] = ("Whether a gyroscope is present.", "true"),
        ["proximity"] = ("Whether a proximity sensor is present.", "true"),
        ["compass"] = ("Whether a compass is present.", "true"),
        ["barometer"] = ("Whether a barometer is present.", "false"),
        ["halla"] = ("Whether a Hall sensor is present; halla is the existing wire property name.", "false"),
        ["ambientLight"] = ("Whether an ambient light sensor is present.", "true"),
        ["camera"] = ("Camera specification text; create/update reject blank text.", "\"50MP\""),
        ["fingerPrint"] = ("Whether fingerprint recognition is supported.", "true"),
        ["faceId"] = ("Whether face recognition is supported.", "false"),
        ["price"] = ("Price amount and currency. The current write policy adds no money rules; no positivity or ISO currency constraint is implied.", null),
        ["amount"] = ("Price amount as a decimal JSON number; trailing zeroes have no semantic significance.", "999.99"),
        ["currency"] = ("Currency text, uppercased by the Money value object; no currency catalogue or three-letter validation is enforced.", "\"PLN\""),
        ["description2"] = ("Second product description section; create/update reject blank text.", "\"Display and connectivity details.\""),
        ["description3"] = ("Third product description section; create/update reject blank text.", "\"Battery and sensor details.\""),
        ["isActive"] = ("False after soft delete. Inactive phones remain readable by ID and are excluded from list/filter/top/by-IDs reads.", "true"),
        ["changedAt"] = ("UTC change time stored in SQL datetime2. The existing history JSON omits the offset; date-time-local describes that wire form, not RFC 3339 date-time.", "\"2026-01-02T12:34:56.1234567\""),
        ["operation"] = ("Numeric history operation: 0 = Inserted, 1 = Updated, 2 = Deleted.", "0"),
        ["minimalPrice"] = ("Optional inclusive lower price bound. Omitted/null means no lower bound; negative values are rejected by the filter policy.", "100"),
        ["maximalPrice"] = ("Optional inclusive upper price bound. Omitted/null means no upper bound; negative values are rejected by the filter policy.", "1500"),
        ["type"] = ("Problem type URI identifying the HTTP error category.", "\"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1\""),
        ["title"] = ("Short safe error title.", "\"Validation failed.\""),
        ["status"] = ("HTTP status, equal to the actual response status.", "400"),
        ["detail"] = ("Safe error explanation; server exceptions do not expose their internal message.", "\"The request failed validation.\""),
        ["instance"] = ("Actual request path, without query parameters.", "\"/mobile-phones\""),
        ["code"] = ("Stable machine-readable error code; see the problem contract for status/cause mappings.", "\"validation_failed\""),
        ["traceId"] = ("Request trace identifier for correlating diagnostics; not a fixed value.", "\"example-trace-id\""),
        ["errors"] = ("Validation errors; an empty array for failures without domain validation errors.", null),
        ["missingProperties"] = ("Serializer-required members missing from the failing object; empty for other causes.", "[\"commonDescription\"]"),
        ["message"] = ("Human-readable possible or actual validation error.", "\"Name cannot be null or whitespace.\""),
        ["entity"] = ("Name of the validated entity or request type.", "\"MobilePhone\""),
        ["actionName"] = ("Command/query name whose flow is described.", "\"CreateMobilePhoneCommand\""),
        ["steps"] = ("Ordered source flow labels. Publishing a flow does not prove every branch was executed.", "[\"Validate amount greater than zero before querying SQL.\"]"),
        ["policyName"] = ("Published validation policy name. Publication alone does not mean an endpoint invokes the policy.", "\"MobilePhonesValidationPolicy\""),
        ["ruleName"] = ("Published rule implementation name.", "\"MobilePhonesCommonDescriptionValidationRule\""),
        ["rules"] = ("Policy rule descriptors, or a rule's possible validation errors, according to the referenced item schema.", null)
    };

    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema concrete)
            return;
        if (context.Type == typeof(Operation))
            concrete.Description = "History operation serialized as an integer: 0 = Inserted, 1 = Updated, 2 = Deleted.";
        if (concrete.Properties is null)
            return;

        var input = context.Type.Namespace?.Contains(".Commands.", StringComparison.Ordinal) == true ||
            context.Type.Namespace == "ProductCatalog.Application.Features.Common" ||
            context.Type == typeof(CommonDescriptionExtrernalDto) || context.Type == typeof(MobilePhoneFilterDto);
        if (input)
        {
            // System.Text.Json ignores unknown request members; do not claim they are rejected.
            concrete.AdditionalPropertiesAllowed = true;
            concrete.Description = "Request object. Required lists reflect serializer-required members, not every domain rule. Unknown JSON members are ignored.";
        }
        if (ResponseTypes.Contains(context.Type))
        {
            concrete.Required ??= new HashSet<string>();
            concrete.Required.UnionWith(concrete.Properties.Keys); // These DTO fields are always serialized.
            concrete.Description = "Response object. All listed fields are present; nullability is specified separately from presence.";
        }

        foreach (var (name, property) in concrete.Properties.ToArray())
        {
            if (!Fields.TryGetValue(name, out var field))
                continue;
            var annotated = property as OpenApiSchema ?? new OpenApiSchema { AllOf = [property] };
            annotated.Description = field.Description;
            if (field.Example is not null)
                annotated.Example = JsonNode.Parse(field.Example);
            concrete.Properties[name] = annotated;
            if (name == "name" && context.Type == typeof(ValidationError))
            {
                annotated.Description = "Validation rule name identifying the error cause.";
                annotated.Example = JsonValue.Create("MobilePhonesCommonDescriptionValidationRule");
            }
            if (name == "changedAt")
                annotated.Format = "date-time-local";
            if (context.Type == typeof(ApiProblemDetails))
            {
                annotated.Type &= ~JsonSchemaType.Null; // ApiProblemResponse always populates the inherited fields.
                if (name is "type" or "instance")
                    annotated.Format = name == "type" ? "uri" : "uri-reference";
            }
        }

        if (context.Type == typeof(MobilePhoneFilterDto))
        {
            concrete.Properties["brand"] = new OpenApiSchema
            {
                Description = "Optional brand: Apple=0, Motorola=1, Samsung=2, Xiaomi=3. Names are case-insensitive; numeric values are accepted. Omitted/null means no brand filter.",
                Example = JsonValue.Create("Samsung"),
                AnyOf =
                [
                    new OpenApiSchema
                    {
                        Type = JsonSchemaType.String | JsonSchemaType.Null,
                        Pattern = "^([Aa][Pp][Pp][Ll][Ee]|[Mm][Oo][Tt][Oo][Rr][Oo][Ll][Aa]|[Ss][Aa][Mm][Ss][Uu][Nn][Gg]|[Xx][Ii][Aa][Oo][Mm][Ii])$"
                    },
                    new OpenApiSchema
                    {
                        Type = JsonSchemaType.Integer, Format = "int32",
                        Enum = Enum.GetValues<MobilePhonesBrand>().Select(value => (JsonNode)JsonValue.Create((int)value)!).ToList()
                    }
                ]
            };
        }
    }
}
