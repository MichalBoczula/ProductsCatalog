using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones;
using ProductCatalog.Domain.Common.Filters;

namespace ProductCatalog.Api.Endpoints
{
    public static class MobilePhonesEndpoints
    {
        public static IEndpointRouteBuilder MapMobilePhonesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/mobile-phones").WithTags("MobilePhones");

            MapMobilePhonesQueries(group);
            MapMobilePhonesCommands(group);

            return group;
        }

        private static void MapMobilePhonesQueries(IEndpointRouteBuilder group)
        {
            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetMobilePhoneByIdQuery(id));

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithSummary("Get mobile phone by Id")
            .WithDescription("Returns the mobile phone details when the Id exists; 404 otherwise.")
            .WithName("GetMobilePhoneById")
            .Produces<MobilePhoneDetailsDto>(StatusCodes.Status200OK)
            .Produces<NotFoundProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapGet("", async ([FromQuery] int amount, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetMobilePhonesQuery(amount));
                return Results.Ok(result);
            })
            .WithSummary("Get mobile phones")
            .WithDescription("Returns a list of mobile phones limited by the provided amount.")
            .WithName("GetMobilePhones")
            .Produces<List<MobilePhoneDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapGet("/{id:guid}/history", async (
                Guid id,
                [FromQuery] int pageNumber,
                [FromQuery] int pageSize,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetMobilePhoneHistoryQuery(id, pageNumber, pageSize));
                return Results.Ok(result);
            })
            .WithSummary("Get mobile phone history")
            .WithDescription("Returns the change history for a mobile phone.")
            .WithName("GetMobilePhoneHistory")
            .Produces<List<MobilePhoneHistoryDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapGet("/top", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetTopMobilePhonesQuery());
                return Results.Ok(result);
            })
            .WithSummary("Get top mobile phones")
            .WithDescription("Returns a list of top mobile phones.")
            .WithName("GetTopMobilePhones")
            .Produces<List<TopMobilePhoneDto>>(StatusCodes.Status200OK)
            .Produces<NotFoundProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapPost("/filter", async (IMediator mediator, MobilePhoneFilterDto mobilePhoneFilterDto) =>
            {
                var result = await mediator.Send(new GetFilteredMobilePhonesQuery(mobilePhoneFilterDto));
                return Results.Ok(result);
            })
            .WithSummary("Get filtered mobile phones")
            .WithDescription("Returns a list of filtered mobile phones.")
            .WithName("GetFiltered MobilePhones")
            .Produces<List<MobilePhoneDto>>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<NotFoundProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
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
            .Produces<MobilePhoneDetailsDto>(StatusCodes.Status201Created)
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
            .Produces<MobilePhoneDetailsDto>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteMobilePhoneCommand(id));
                return Results.Ok(result);
            })
            .WithSummary("Delete mobile phone")
            .WithDescription("Soft deletes a mobile phone and returns the deactivated resource.")
            .WithName("DeleteMobilePhone")
            .Produces<MobilePhoneDetailsDto>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        }
    }
}
