using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;

namespace ProductCatalog.Application.Features.Currencies.Queries.GetCurrencies
{
    public sealed record GetCurrenciesQuery : IRequest<IReadOnlyList<CurrencyDto>>; 
}
