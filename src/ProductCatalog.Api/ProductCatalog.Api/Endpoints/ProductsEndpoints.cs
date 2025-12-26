using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.RemoveProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
using ProductCatalog.Application.Features.Products.Queries.GetProductById;
using ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId;


namespace ProductCatalog.Api.Endpoints
{
    public static class ProductsEndpoints
    {
        public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/products").WithTags("Products");

            MapProductsQueries(group);
            MapProductsCommands(group);

            return group;
        }

        private static void MapProductsQueries(IEndpointRouteBuilder group)
        {
            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProductByIdQuery(id));

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetProductById")
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .Produces<NotFoundProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a product by ID",
                Description = "Returns the product details when the ID exists; 404 otherwise."
            });

            group.MapGet("/categories/{categoryId:guid}", async (Guid categoryId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId));

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetProductByCategoryId")
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .Produces<NotFoundProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a products by categoryId",
                Description = "Returns the products lists when categoryId exists; 404 otherwise."
            });
        }

        private static void MapProductsCommands(IEndpointRouteBuilder group)
        {
            group.MapPost("", async (CreateProductExternalDto product, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateProductCommand(product));
                return Results.Created($"/products/{result.Id}", result);
            })
            .WithName("CreateProduct")
            .Produces<ProductDto>(StatusCodes.Status201Created)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create product",
                Description = "Creates a new product and returns the created resource.",
            });

            group.MapPut("/{id:guid}", async (Guid id, UpdateProductExternalDto product, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateProductCommand(id, product));
                return Results.Ok(result);
            })
            .WithName("UpdateProduct")
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update product",
                Description = "Updates an existing product and returns the updated resource.",
            });

            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new RemoveProductCommand(id));
                return Results.Ok(result);
            })
           .WithName("RemoveProduct")
           .Produces<ProductDto>(StatusCodes.Status200OK)
           .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
           .WithOpenApi(operation => new(operation)
           {
               Summary = "Delete product",
               Description = "Soft deletes a product and returns the deactivated resource.",
           });
        }
    }
}
