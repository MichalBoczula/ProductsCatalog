
using MediatR;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;

namespace ProductCatalog.Api.Endpoints
{
    public static class CategoriesEndpoints
    {
        public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
        {
            MapCategoriesQueries(app);
            MapCategoriesCommands(app);

            return app;
        }

        private static void MapCategoriesCommands(IEndpointRouteBuilder app)
        {
            app.MapPost("/categories", async (CreateCategoryExternalDto category, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateCategoryCommand(category));
                return Results.Created($"/categories/{result.Id}", result);
            })
            .WithName("CreateCategory")
            .WithOpenApi();
        }

        private static void MapCategoriesQueries(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories/{id:guid}", async (Guid Id, IMediator mediator) =>
            {
                var result = await mediator.Send(new object());

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetCategorybyId")
            .WithOpenApi();
        }
    }
}
