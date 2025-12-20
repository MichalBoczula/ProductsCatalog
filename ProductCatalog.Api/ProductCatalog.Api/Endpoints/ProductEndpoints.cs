using MediatR;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

namespace ProductCatalog.Api.Endpoints
{
    public static class ProductEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
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
                var result = await mediator.Send(new UpdateProductCommand(product));
                return Results.Created($"/products/{result.Id}", result);
            })
                .WithName("UpdateProduct")
                .WithOpenApi();

            return app;
        }
    }
}
