using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
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

            group.MapGet("/currencies/create", ([FromServices] IFlowDescriber<CreateCurrencyCommand> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeCreateCurrencyFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the create currency request flow",
                Description = "Returns the ordered steps executed when handling CreateCurrencyCommand."
            });

            group.MapGet("/currencies/update", ([FromServices] IFlowDescriber<UpdateCurrencyCommand> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeUpdateCurrencyFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the update currency request flow",
                Description = "Returns the ordered steps executed when handling UpdateCurrencyCommand."
            });

            group.MapGet("/currencies/delete", ([FromServices] IFlowDescriber<DeleteCurrencyCommand> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeDeleteCurrencyFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the delete currency request flow",
                Description = "Returns the ordered steps executed when handling DeleteCurrencyCommand."
            });

            return group;
        }
    }
}