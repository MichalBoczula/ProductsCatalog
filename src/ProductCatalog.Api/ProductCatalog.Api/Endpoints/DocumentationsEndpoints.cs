using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.RemoveProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
using ProductCatalog.Application.Features.Currencies.Queries.GetCurrencies;
using ProductCatalog.Application.Features.Categories.Queries.GetCategories;
using ProductCatalog.Application.Features.Categories.Queries.GetCategoryById;
using ProductCatalog.Application.Features.Products.Queries.GetProductById;
using ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using ProductCatalog.Application.Features.Categories.Commands.DeleteCategory;

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

            group.MapGet("/products/get-by-id", ([FromServices] IFlowDescriber<GetProductByIdQuery> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeGetProductByIdFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the get product by id request flow",
                Description = "Returns the ordered steps executed when handling GetProductByIdQuery."
            });

            group.MapGet("/products/get-by-category", ([FromServices] IFlowDescriber<GetProductsByCategoryIdQuery> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeGetProductsByCategoryIdFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the get products by category request flow",
                Description = "Returns the ordered steps executed when handling GetProductsByCategoryIdQuery."
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

            group.MapGet("/currencies/get", ([FromServices] IFlowDescriber<GetCurrenciesQuery> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeGetCurrenciesFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the get currencies request flow",
                Description = "Returns the ordered steps executed when handling GetCurrenciesQuery."
            });

            group.MapGet("/categories/create", ([FromServices] IFlowDescriber<CreateCategoryCommand> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeCreateCategoryFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the create category request flow",
                Description = "Returns the ordered steps executed when handling CreateCategoryCommand."
            });

            group.MapGet("/categories/update", ([FromServices] IFlowDescriber<UpdateCategoryCommand> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeUpdateCategoryFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the update category request flow",
                Description = "Returns the ordered steps executed when handling UpdateCategoryCommand."
            });

            group.MapGet("/categories/delete", ([FromServices] IFlowDescriber<DeleteCategoryCommand> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeDeleteCategoryFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the delete category request flow",
                Description = "Returns the ordered steps executed when handling DeleteCategoryCommand."
            });

            group.MapGet("/categories/get", ([FromServices] IFlowDescriber<GetCategoriesQuery> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeGetCategoriesFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the get categories request flow",
                Description = "Returns the ordered steps executed when handling GetCategoriesQuery."
            });

            group.MapGet("/categories/get-by-id", ([FromServices] IFlowDescriber<GetCategoryByIdQuery> flowDescriber) =>
            {
                var description = flowDescriber.DescribeFlow(default!);
                return Results.Ok(description);
            })
            .WithName("DescribeGetCategoryByIdFlow")
            .Produces<FlowDescription>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Describe the get category by id request flow",
                Description = "Returns the ordered steps executed when handling GetCategoryByIdQuery."
            });

            return group;
        }
    }
}
