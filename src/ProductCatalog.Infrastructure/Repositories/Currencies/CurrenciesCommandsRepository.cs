using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Infrastructure.Repositories.Currencies
{
    internal class CurrenciesCommandsRepository : ICurrenciesCommandsRepository
    {
        private readonly ProductsContext _context;

        public CurrenciesCommandsRepository(ProductsContext context)
        {
            _context = context;
        }

        public void Add(Currency currency)
        {
            _context.Currencies.Add(currency);
        }

        public void Update(Currency currency)
        {
            _context.Currencies.Update(currency);
        }

        public async Task<Currency?> GetCurrencyById(Guid currencyId, CancellationToken cancellationToken)
        {
            var category = await _context.Currencies.FirstOrDefaultAsync(c => c.Id == currencyId, cancellationToken);
            return category;
        }

        public void WriteHistory(CurrenciesHistory entity)
        {
            _context.CurrenciesHistories.Add(entity);
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}