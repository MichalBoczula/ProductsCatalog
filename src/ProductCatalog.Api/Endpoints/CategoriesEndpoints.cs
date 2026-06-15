using MediatR;
using Microsoft.AspNetCore.Mvc;
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
           .WithSummary("Get all categories")
           .WithDescription("Returns all categories; 404 if no categories are found.")
           .Produces<IReadOnlyList<CategoryDto>>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoryByIdQuery(id));

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetCategoryById")
            .WithDescription("Returns the category when the ID exists; 404 otherwise.")
            .WithSummary("Get category by ID")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .Produces<NotFoundProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        }

        private static void MapCategoriesCommands(IEndpointRouteBuilder group)
        {
            group.MapPost("", async (CreateCategoryExternalDto category, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateCategoryCommand(category));
                return Results.Created($"/categories/{result.Id}", result);
            })
            .WithName("CreateCategory")
            .WithDescription("Creates a new category and returns the created resource.")
            .WithSummary("Create category")
            .Produces<CategoryDto>(StatusCodes.Status201Created)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}", async (Guid id, UpdateCategoryExternalDto category, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateCategoryCommand(id, category));
                return Results.Ok(result);
            })
            .WithName("UpdateCategory")
            .WithSummary("Update category")
            .WithDescription("Updates an existing category and returns the updated resource.")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteCategoryCommand(id));
                return Results.Ok(result);
            })
           .WithName("RemoveCategory")
           .WithSummary("Delete category")
           .WithDescription("Soft deletes a category and returns the deactivated resource.")
           .Produces<CategoryDto>(StatusCodes.Status200OK)
           .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        }
    }
}
