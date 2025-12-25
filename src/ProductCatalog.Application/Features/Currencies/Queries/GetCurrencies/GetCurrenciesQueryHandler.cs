using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;

namespace ProductCatalog.Application.Features.Currencies.Queries.GetCurrencies
{
    internal sealed class GetCurrenciesQueryHandler
        (ICurrenciesQueriesRepository _currenciesQueriesRepository)
        : IRequestHandler<GetCurrenciesQuery, IReadOnlyList<CurrencyDto>>
    {
        public async Task<IReadOnlyList<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencies = await _currenciesQueriesRepository.GetCurrencies(cancellationToken);
            return currencies.Adapt<IReadOnlyList<CurrencyDto>>();
        }
    }
}
