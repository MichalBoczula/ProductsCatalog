using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Endpoints
{
    public static class DocumentationsEndpoints
    {
        public static IEndpointRouteBuilder MapDocumentationsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/products-documentation").WithTags("Documentation");

            group.MapGet("/flow", (
                [FromServices] IFlowDescriber<CreateMobilePhoneCommand> createMobilePhoneFlowDescriber,
                [FromServices] IFlowDescriber<UpdateMobilePhoneCommand> updateMobilePhoneFlowDescriber,
                [FromServices] IFlowDescriber<DeleteMobilePhoneCommand> deleteMobilePhoneFlowDescriber,
                [FromServices] IFlowDescriber<GetMobilePhoneByIdQuery> getMobilePhoneByIdFlowDescriber,
                [FromServices] IFlowDescriber<GetMobilePhoneByIdsQuery> getMobilePhoneByIdsFlowDescriber,
                [FromServices] IFlowDescriber<GetMobilePhoneHistoryQuery> getMobilePhoneHistoryFlowDescriber,
                [FromServices] IFlowDescriber<GetMobilePhonesQuery> getMobilePhonesFlowDescriber,
                [FromServices] IFlowDescriber<GetFilteredMobilePhonesQuery> getFilteredMobilePhonesFlowDescriber,
                [FromServices] IFlowDescriber<GetTopMobilePhonesQuery> getTopMobilePhonesFlowDescriber) =>
            {
                var descriptions = new List<FlowDescription>
                {
                    createMobilePhoneFlowDescriber.DescribeFlow(default!),
                    updateMobilePhoneFlowDescriber.DescribeFlow(default!),
                    deleteMobilePhoneFlowDescriber.DescribeFlow(default!),
                    getMobilePhoneByIdFlowDescriber.DescribeFlow(default!),
                    getMobilePhoneByIdsFlowDescriber.DescribeFlow(default!),
                    getMobilePhoneHistoryFlowDescriber.DescribeFlow(default!),
                    getMobilePhonesFlowDescriber.DescribeFlow(default!),
                    getFilteredMobilePhonesFlowDescriber.DescribeFlow(default!),
                    getTopMobilePhonesFlowDescriber.DescribeFlow(default!),
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
