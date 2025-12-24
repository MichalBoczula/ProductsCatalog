using MediatR;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Queries.GetCategories;
using ProductCatalog.Application.Features.Categories.Queries.GetCategoryById;

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
            app.MapGet("/categories", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoriesQuery());

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
           .WithName("GetCategories")
           .WithOpenApi();

            app.MapGet("/categories/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoryByIdQuery(id));

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetCategoryById")
            .WithOpenApi();
        }
    }
}
