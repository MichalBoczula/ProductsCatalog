using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;

namespace ProductCatalog.Api.Endpoints
{
    public static class MobilePhonesEndpoints
    {
        public static IEndpointRouteBuilder MapMobilePhonesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/mobile-phones").WithTags("MobilePhones");

            MapMobilePhonesCommands(group);

            return group;
        }

        private static void MapMobilePhonesCommands(IEndpointRouteBuilder group)
        {
            group.MapPost("", async (CreateMobilePhoneExternalDto mobilePhone, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateMobilePhoneCommand(mobilePhone));
                return Results.Created($"/mobile-phones/{result.Id}", result);
            })
            .WithSummary("Create mobile phone")
            .WithDescription("Creates a new mobile phone and returns the created resource.")
            .WithName("CreateMobilePhone")
            .Produces<MobilePhoneDto>(StatusCodes.Status201Created)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}", async (Guid id, UpdateMobilePhoneExternalDto mobilePhone, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateMobilePhoneCommand(id, mobilePhone));
                return Results.Ok(result);
            })
            .WithSummary("Update mobile phone")
            .WithDescription("Updates an existing mobile phone and returns the updated resource.")
            .WithName("UpdateMobilePhone")
            .Produces<MobilePhoneDto>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        }
    }
}
