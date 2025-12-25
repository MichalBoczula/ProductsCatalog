using MediatR;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
using ProductCatalog.Application.Features.Currencies.Queries.GetCurrencies;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.RemoveProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;

namespace ProductCatalog.Api.Endpoints
{
    public static class CurrenciesEndpoints
    {
        public static IEndpointRouteBuilder MapCurrenciesEndpoints(this IEndpointRouteBuilder app)
        {
            MapCurenciesQueries(app);
            MapCurrenciesCommands(app);

            return app;
        }

        private static void MapCurenciesQueries(IEndpointRouteBuilder app)
        {
            app.MapGet("/currencies", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCurrenciesQuery());

                return result is null ?
                    Results.NotFound()
                  : Results.Ok(result);
            })
            .WithName("GetCurrencies")
            .WithOpenApi();
        }

        private static void MapCurrenciesCommands(IEndpointRouteBuilder app)
        {
            app.MapPost("/currencies", async (CreateCurrencyExternalDto currency, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateCurrencyCommand(currency));
                return Results.Created($"/currencies/{result.Id}", result);
            })
            .WithName("CreateCurrency")
            .WithOpenApi();

            app.MapPut("/currencies/{id:guid}", async (Guid id, UpdateCurrencyExternalDto currency, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateCurrencyCommand(id, currency));
                return Results.Ok(result);
            })
           .WithName("UpdateCurrency")
           .WithOpenApi();

            app.MapDelete("/currencies/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteCurrencyCommand(id));
                return Results.Ok(result);
            })
           .WithName("RemoveCurrency")
           .WithOpenApi();
        }
    }
}
