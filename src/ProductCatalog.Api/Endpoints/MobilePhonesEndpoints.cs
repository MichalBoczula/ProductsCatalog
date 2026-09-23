using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Common.Pagination;

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
            group.MapPost("/by-ids", async (List<Guid> ids, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetMobilePhoneByIdsQuery(ids), cancellationToken);

                return Results.Ok(result);
            })
            .WithSummary("Get mobile phones by Ids")
            .WithDescription("Returns the mobile phones matching the provided Ids.")
            .WithName("GetMobilePhonesByIds")
            .Produces<IReadOnlyList<MobilePhoneDto>>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetMobilePhoneByIdQuery(id), cancellationToken);

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithSummary("Get mobile phone by Id")
            .WithDescription("Returns the mobile phone details when the Id exists; 404 otherwise.")
            .WithName("GetMobilePhoneById")
            .Produces<MobilePhoneDetailsDto>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            group.MapGet("", async ([FromQuery] int amount, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetMobilePhonesQuery(amount), cancellationToken);
                return Results.Ok(result);
            })
            .WithSummary("Get mobile phones")
            .WithDescription("Returns a list of mobile phones limited by the provided amount.")
            .WithName("GetMobilePhones")
            .Produces<List<MobilePhoneDto>>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            group.MapGet("/{id:guid}/history", async (
                Guid id,
                [FromQuery] int pageNumber,
                [FromQuery] int pageSize,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetMobilePhoneHistoryQuery(id, new PaginationParameters(pageNumber, pageSize)), cancellationToken);
                return Results.Ok(result);
            })
            .WithSummary("Get mobile phone history")
            .WithDescription("Returns the change history for a mobile phone.")
            .WithName("GetMobilePhoneHistory")
            .Produces<List<MobilePhoneHistoryDto>>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            group.MapGet("/top", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetTopMobilePhonesQuery(), cancellationToken);
                return Results.Ok(result);
            })
            .WithSummary("Get top mobile phones")
            .WithDescription("Returns a list of top mobile phones.")
            .WithName("GetTopMobilePhones")
            .Produces<List<TopMobilePhoneDto>>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            group.MapPost("/filter", async (IMediator mediator, MobilePhoneFilterDto mobilePhoneFilterDto, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetFilteredMobilePhonesQuery(mobilePhoneFilterDto), cancellationToken);
                return Results.Ok(result);
            })
            .WithSummary("Get filtered mobile phones")
            .WithDescription("Returns a list of filtered mobile phones.")
            .WithName("GetFiltered MobilePhones")
            .Produces<List<MobilePhoneDto>>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }

        private static void MapMobilePhonesCommands(IEndpointRouteBuilder group)
        {
            group.MapPost("", async (CreateMobilePhoneExternalDto mobilePhone, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new CreateMobilePhoneCommand(mobilePhone), cancellationToken);
                return Results.Created($"/mobile-phones/{result.Id}", result);
            })
            .WithSummary("Create mobile phone")
            .WithDescription("Creates a new mobile phone and returns the created resource.")
            .WithName("CreateMobilePhone")
            .Produces<MobilePhoneDetailsDto>(StatusCodes.Status201Created)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            group.MapPut("/{id:guid}", async (Guid id, UpdateMobilePhoneExternalDto mobilePhone, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new UpdateMobilePhoneCommand(id, mobilePhone), cancellationToken);
                return Results.Ok(result);
            })
            .WithSummary("Update mobile phone")
            .WithDescription("Updates a mobile phone. An unchanged request is a successful no-op; a concurrent write returns 409, or 404 if the row was removed during the request.")
            .WithName("UpdateMobilePhone")
            .Produces<MobilePhoneDetailsDto>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new DeleteMobilePhoneCommand(id), cancellationToken);
                return Results.Ok(result);
            })
            .WithSummary("Delete mobile phone")
            .WithDescription("Soft deletes a mobile phone. Repeating a delete is a successful no-op; an overlapping update/delete returns 409, or 404 if the row was removed during the request.")
            .WithName("DeleteMobilePhone")
            .Produces<MobilePhoneDetailsDto>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")
            .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
