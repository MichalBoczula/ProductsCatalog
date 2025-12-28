using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.RemoveProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

namespace ProductCatalog.Api.Endpoints
{
    public static class DocumentationsEndpoints
    {
        public static IEndpointRouteBuilder MapDocumentationsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/documentation").WithTags("Documentation");

            group.MapGet("/products/create", ([FromServices] IFlowDescriber<CreateProductCommand> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeCreateProductFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the create product request flow",
                Description = "Returns the ordered steps executed when handling CreateProductCommand."
            });

            group.MapGet("/products/update", ([FromServices] IFlowDescriber<UpdateProductCommand> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeUpdateProductFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the update product request flow",
                Description = "Returns the ordered steps executed when handling UpdateProductCommand."
            });

            group.MapGet("/products/remove", ([FromServices] IFlowDescriber<RemoveProductCommand> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeRemoveProductFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the remove product request flow",
                Description = "Returns the ordered steps executed when handling RemoveProductCommand."
            });

            return group;
        }
    }
}
