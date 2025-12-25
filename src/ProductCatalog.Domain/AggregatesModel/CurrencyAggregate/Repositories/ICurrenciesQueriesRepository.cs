using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories
{
    public interface ICurrenciesQueriesRepository
    {
        Task<IReadOnlyList<CurrencyReadModel>> GetCurrencies(Guid categoryId, CancellationToken ct);
    }
}
