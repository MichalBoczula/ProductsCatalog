using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;

namespace ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency
{
    public sealed record DeleteCurrencyCommand(Guid currencyId) : IRequest<CurrencyDto>;
}
