using Mapster;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Features.Currencies.Queries.GetCurrencies
{
    internal sealed class GetCurrenciesQueryFlowDescribtor : FlowDescriberBase<GetCurrenciesQuery>
    {
        [FlowStep(1)]
        public Task<IReadOnlyList<CurrencyReadModel>> GetCurrencies(ICurrenciesQueriesRepository currenciesQueriesRepository, CancellationToken cancellationToken)
        {
            return currenciesQueriesRepository.GetCurrencies(cancellationToken);
        }

        [FlowStep(2)]
        public IReadOnlyList<CurrencyDto> MapCurrenciesToDto(IReadOnlyList<CurrencyReadModel> currencies)
        {
            return currencies.Adapt<IReadOnlyList<CurrencyDto>>();
        }
    }
}
