using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;

namespace ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency
{
    public sealed record UpdateCurrencyCommand(Guid currencyId, UpdateCurrencyExternalDto currency) : IRequest<CurrencyDto>;
}
