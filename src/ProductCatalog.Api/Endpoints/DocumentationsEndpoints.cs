using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Configuration.Common;
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
    [FlowOutcome("products.branch.docs.flows", "Registered flow descriptors succeed.", "Return all nine source flow descriptions as 200; publication is not branch execution evidence.", Stage = "documentation")]
    [FlowOutcome("products.branch.docs.flow-failure", "A flow descriptor throws.", "Return safe internal_error 500 without exposing the descriptor exception.", Stage = "documentation")]
    [FlowOutcome("products.branch.docs.policies", "Registered validation descriptors succeed.", "Return all five published policies as 200, including the standalone hardware policy.", Stage = "documentation")]
    [FlowOutcome("products.branch.docs.policy-failure", "A validation descriptor throws.", "Return safe internal_error 500 without exposing the descriptor exception.", Stage = "documentation")]
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
            .WithDescription("Returns declared ordered source flow steps. Conditional paths may skip steps; publishing a description is not runtime execution evidence.")
            .Produces<IReadOnlyCollection<FlowDescription>>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            group.MapGet("/validation-policies", ([FromServices] IEnumerable<IValidationPolicyDescriptorProvider> validationPolicyProviders) =>
                Results.Ok(validationPolicyProviders
                    .Select(provider => provider.Describe())
                    .OrderBy(descriptor => descriptor.PolicyName)
                    .ToList()))
            .WithName("DescribeValidationPolicies")
            .WithSummary("Describe all validation policies")
            .WithDescription("Returns published policies, rules and possible errors, including standalone policies not invoked by catalogue endpoints. The generated source catalogue records invocation scope.")
            .Produces<IReadOnlyCollection<ValidationPolicyDescriptor>>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            return group;
        }
    }
}
