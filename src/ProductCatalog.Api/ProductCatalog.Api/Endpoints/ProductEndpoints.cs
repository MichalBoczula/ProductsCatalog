using MediatR;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
using ProductCatalog.Application.Features.Products.Queries.GetProductById;
using ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId;

namespace ProductCatalog.Api.Endpoints
{
    public static class ProductEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            MapProductsQueries(app);
            MapProductsCommands(app);

            return app;
        }

        private static void MapProductsQueries(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProductByIdQuery(id));

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetProductById")
            .WithOpenApi();

            app.MapGet("/products/categories/{categoryId:guid}", async (Guid categoryId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId));

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetProductByCategoryId")
            .WithOpenApi();
        }

        private static void MapProductsCommands(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductExternalDto product, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateProductCommand(product));
                return Results.Created($"/products/{result.Id}", result);
            })
            .WithName("CreateProduct")
            .WithOpenApi();

            app.MapPut("/products/{id:guid}", async (Guid id, UpdateProductExternalDto product, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateProductCommand(id, product));
                return Results.Created($"/products/{result.Id}", result);
            })
            .WithName("UpdateProduct")
            .WithOpenApi();
        }
    }
}
