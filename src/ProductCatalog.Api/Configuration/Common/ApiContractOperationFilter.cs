using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Nodes;

namespace ProductCatalog.Api.Configuration.Common;

public sealed class ApiContractOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters ?? [])
        {
            parameter.Description = parameter.Name switch
            {
                "id" => "Phone identifier in UUID form; this is not a history-row identifier.",
                "amount" => "Required positive maximum number of active phones to return; ordered by name, then Id.",
                "pageNumber" => "Required one-based history page number; must be greater than zero.",
                "pageSize" => "Required positive history page size; no upper bound is currently enforced.",
                _ => parameter.Description
            };
            if (parameter is OpenApiParameter concrete)
                concrete.Example = parameter.Name == "id"
                    ? JsonValue.Create("4f769bfe-bb85-4ba3-b705-36042c1049fc")
                    : JsonValue.Create(parameter.Name == "pageNumber" ? 1 : 15);
        }

        if (operation.OperationId == "CreateMobilePhone" && operation.Responses?["201"] is OpenApiResponse created)
        {
            created.Headers = new Dictionary<string, IOpenApiHeader>
            {
                ["Location"] = new OpenApiHeader
                {
                    Required = true,
                    Description = "Relative path to the created phone; GET returns the same resource identifier.",
                    Schema = new OpenApiSchema { Type = JsonSchemaType.String, Format = "uri-reference" },
                    Example = JsonValue.Create("/mobile-phones/4f769bfe-bb85-4ba3-b705-36042c1049fc")
                }
            };
        }
    }
}
