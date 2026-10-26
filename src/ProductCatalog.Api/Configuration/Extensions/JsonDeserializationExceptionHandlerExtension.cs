using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.FlowDescriptors.Common;

namespace ProductCatalog.Api.Configuration.Extensions;

[FlowOutcome("products.branch.http.json-binding", "JSON syntax, scalar type, required member or non-null value-type binding fails before MediatR.", "Return invalid_json 400 with safe problem details; the business flow is not entered and no current/history write occurs.", Stage = "framework")]
public static class JsonDeserializationExceptionHandlerExtension
{
    public static async Task HandleJsonDeserializationException(
        this HttpContext context,
        CancellationToken cancellationToken)
    {
        var missing = await RequiredJsonProperties.FindMissingAsync(context, cancellationToken);
        var detail = missing.Names.Count > 0
            ? $"JSON payload for {missing.TypeName} is missing required properties: {string.Join(", ", missing.Names)}."
            : "The request body is not valid JSON.";

        await ApiProblemResponse.WriteAsync(
            context,
            StatusCodes.Status400BadRequest,
            "invalid_json",
            "Invalid JSON payload.",
            detail,
            cancellationToken,
            missingProperties: missing.Names);
    }
}
