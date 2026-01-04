using System.Collections.Generic;
using System.Linq;
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
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones;
using ProductCatalog.Application.Features.Products.Queries.GetProductById;
using ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using ProductCatalog.Application.Features.Categories.Commands.DeleteCategory;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Endpoints
{
    public static class DocumentationsEndpoints
    {
        public static IEndpointRouteBuilder MapDocumentationsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/documentation").WithTags("Documentation");

            group.MapGet("/flow", (
                [FromServices] IFlowDescriber<CreateProductCommand> createProductFlowDescriber,
                [FromServices] IFlowDescriber<UpdateProductCommand> updateProductFlowDescriber,
                [FromServices] IFlowDescriber<RemoveProductCommand> removeProductFlowDescriber,
                [FromServices] IFlowDescriber<CreateCurrencyCommand> createCurrencyFlowDescriber,
                [FromServices] IFlowDescriber<UpdateCurrencyCommand> updateCurrencyFlowDescriber,
                [FromServices] IFlowDescriber<DeleteCurrencyCommand> deleteCurrencyFlowDescriber,
                [FromServices] IFlowDescriber<CreateCategoryCommand> createCategoryFlowDescriber,
                [FromServices] IFlowDescriber<UpdateCategoryCommand> updateCategoryFlowDescriber,
                [FromServices] IFlowDescriber<DeleteCategoryCommand> deleteCategoryFlowDescriber,
                [FromServices] IFlowDescriber<CreateMobilePhoneCommand> createMobilePhoneFlowDescriber,
                [FromServices] IFlowDescriber<UpdateMobilePhoneCommand> updateMobilePhoneFlowDescriber,
                [FromServices] IFlowDescriber<DeleteMobilePhoneCommand> deleteMobilePhoneFlowDescriber,
                [FromServices] IFlowDescriber<GetMobilePhoneByIdQuery> getMobilePhoneByIdFlowDescriber,
                [FromServices] IFlowDescriber<GetMobilePhonesQuery> getMobilePhonesFlowDescriber,
                [FromServices] IFlowDescriber<GetCurrenciesQuery> getCurrenciesFlowDescriber,
                [FromServices] IFlowDescriber<GetCategoriesQuery> getCategoriesFlowDescriber,
                [FromServices] IFlowDescriber<GetCategoryByIdQuery> getCategoryByIdFlowDescriber,
                [FromServices] IFlowDescriber<GetProductByIdQuery> getProductByIdFlowDescriber,
                [FromServices] IFlowDescriber<GetProductsByCategoryIdQuery> getProductsByCategoryIdFlowDescriber) =>
            {
                var descriptions = new List<FlowDescription>
                {
                    createProductFlowDescriber.DescribeFlow(default!),
                    updateProductFlowDescriber.DescribeFlow(default!),
                    removeProductFlowDescriber.DescribeFlow(default!),
                    createCurrencyFlowDescriber.DescribeFlow(default!),
                    updateCurrencyFlowDescriber.DescribeFlow(default!),
                    deleteCurrencyFlowDescriber.DescribeFlow(default!),
                    createCategoryFlowDescriber.DescribeFlow(default!),
                    updateCategoryFlowDescriber.DescribeFlow(default!),
                    deleteCategoryFlowDescriber.DescribeFlow(default!),
                    createMobilePhoneFlowDescriber.DescribeFlow(default!),
                    updateMobilePhoneFlowDescriber.DescribeFlow(default!),
                    deleteMobilePhoneFlowDescriber.DescribeFlow(default!),
                    getMobilePhoneByIdFlowDescriber.DescribeFlow(default!),
                    getMobilePhonesFlowDescriber.DescribeFlow(default!),
                    getCurrenciesFlowDescriber.DescribeFlow(default!),
                    getCategoriesFlowDescriber.DescribeFlow(default!),
                    getCategoryByIdFlowDescriber.DescribeFlow(default!),
                    getProductByIdFlowDescriber.DescribeFlow(default!),
                    getProductsByCategoryIdFlowDescriber.DescribeFlow(default!)
                };

                return Results.Ok(descriptions);
            })
            .WithName("DescribeAllFlows")
            .WithSummary("Describe all request flows")
            .WithDescription("Returns the ordered steps executed when handling every documented request.")
            .Produces<IReadOnlyCollection<FlowDescription>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapGet("/validation-policies", ([FromServices] IEnumerable<IValidationPolicyDescriptorProvider> validationPolicyProviders) =>
                Results.Ok(validationPolicyProviders
                    .Select(provider => provider.Describe())
                    .OrderBy(descriptor => descriptor.PolicyName)
                    .ToList()))
            .WithName("DescribeValidationPolicies")
            .WithSummary("Describe all validation policies")
            .WithDescription("Returns validation policies with their rules and possible validation errors.")
            .Produces<IReadOnlyCollection<ValidationPolicyDescriptor>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
