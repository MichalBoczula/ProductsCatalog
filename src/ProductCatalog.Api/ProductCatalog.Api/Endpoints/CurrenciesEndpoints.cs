using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
using ProductCatalog.Application.Features.Currencies.Queries.GetCurrencies;

namespace ProductCatalog.Api.Endpoints
{
    public static class CurrenciesEndpoints
    {
        public static IEndpointRouteBuilder MapCurrenciesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/currencies").WithTags("Currencies");

            MapCurenciesQueries(group);
            MapCurrenciesCommands(group);

            return group;
        }

        private static void MapCurenciesQueries(IEndpointRouteBuilder group)
        {
            group.MapGet("", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCurrenciesQuery());

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetCurrencies")
            .WithSummary("List currencies")
            .WithDescription("Returns all currencies; 404 if no currencies are found.")
            .Produces<IReadOnlyList<CurrencyDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        }

        private static void MapCurrenciesCommands(IEndpointRouteBuilder group)
        {
            group.MapPost("", async (CreateCurrencyExternalDto currency, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateCurrencyCommand(currency));
                return Results.Created($"/currencies/{result.Id}", result);
            })
            .WithName("CreateCurrency")
            .WithSummary("Create currency")
            .WithDescription("Creates a new currency and returns the created resource.")
            .Produces<CurrencyDto>(StatusCodes.Status201Created)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}", async (Guid id, UpdateCurrencyExternalDto currency, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateCurrencyCommand(id, currency));
                return Results.Ok(result);
            })
           .WithName("UpdateCurrency")
           .WithSummary("Update currency")
           .WithDescription("Updates an existing currency and returns the updated resource.")
           .Produces<CurrencyDto>(StatusCodes.Status200OK)
           .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteCurrencyCommand(id));
                return Results.Ok(result);
            })
           .WithName("RemoveCurrency")
           .WithSummary("Delete currency")
           .WithDescription("Deletes a currency and returns the deactivated resource.")
           .Produces<CurrencyDto>(StatusCodes.Status200OK)
           .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        }
    }
}
