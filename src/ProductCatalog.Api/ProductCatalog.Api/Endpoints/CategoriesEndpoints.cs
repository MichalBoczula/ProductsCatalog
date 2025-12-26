using MediatR;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Commands.DeleteCategory;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using ProductCatalog.Application.Features.Categories.Queries.GetCategories;
using ProductCatalog.Application.Features.Categories.Queries.GetCategoryById;

namespace ProductCatalog.Api.Endpoints
{
    public static class CategoriesEndpoints
    {
        public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/categories").WithTags("Categories");

            MapCategoriesQueries(group);
            MapCategoriesCommands(group);

            return group;
        }

        private static void MapCategoriesQueries(IEndpointRouteBuilder group)
        {
            group.MapGet("", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoriesQuery());

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
           .WithName("GetCategories")
           .Produces<IReadOnlyList<CategoryDto>>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound)
           .WithOpenApi(operation => new(operation)
           {
               Summary = "List categories",
               Description = "Returns all categories; 404 if no categories are found.",
           });

            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoryByIdQuery(id));

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetCategoryById")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get category by ID",
                Description = "Returns the category when the ID exists; 404 otherwise.",
            });
        }

        private static void MapCategoriesCommands(IEndpointRouteBuilder group)
        {
            group.MapPost("", async (CreateCategoryExternalDto category, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateCategoryCommand(category));
                return Results.Created($"/categories/{result.Id}", result);
            })
            .WithName("CreateCategory")
            .Produces<CategoryDto>(StatusCodes.Status201Created)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create category",
                Description = "Creates a new category and returns the created resource.",
            });

            group.MapPut("/{id:guid}", async (Guid id, UpdateCategoryExternalDto category, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateCategoryCommand(id, category));
                return Results.Ok(result);
            })
            .WithName("UpdateCategory")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update category",
                Description = "Updates an existing category and returns the updated resource.",
            });

            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteCategoryCommand(id));
                return Results.Ok(result);
            })
           .WithName("RemoveCategory")
           .Produces<CategoryDto>(StatusCodes.Status200OK)
           .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
           .WithOpenApi(operation => new(operation)
           {
               Summary = "Delete category",
               Description = "Soft deletes a category and returns the deactivated resource.",
           });
        }
    }
}
