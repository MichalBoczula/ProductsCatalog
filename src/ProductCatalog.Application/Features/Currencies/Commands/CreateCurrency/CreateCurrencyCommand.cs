using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;

namespace ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency
{
    public sealed record CreateCurrencyCommand(CreateCurrencyExternalDto currency) : IRequest<CurrencyDto>;
}
