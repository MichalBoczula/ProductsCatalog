using ProductCatalog.Domain.AggregatesModel.ProductAggregate;

namespace ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories
{
    public interface ICurrencyCommandsRepository
    {
        Task Add(Currency currency, CancellationToken cancellationToken);
        Task Update(Currency currency, CancellationToken cancellationToken);
        Task<Currency?> GetCurrencyById(Guid currencyId, CancellationToken cancellationToken);
    }
}
