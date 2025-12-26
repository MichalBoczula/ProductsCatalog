using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;

namespace ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories
{
    public interface ICurrenciesCommandsRepository
    {
        void Add(Currency currency);
        void Update(Currency currency);
        Task<Currency?> GetCurrencyById(Guid currencyId, CancellationToken cancellationToken);
        void WriteHistory(CurrenciesHistory entity);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
