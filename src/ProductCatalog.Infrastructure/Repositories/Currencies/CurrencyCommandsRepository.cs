using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Infrastructure.Repositories.Currencies
{
    internal class CurrencyCommandsRepository : ICurrencyCommandsRepository
    {
        private readonly ProductsContext _context;

        public CurrencyCommandsRepository(ProductsContext context)
        {
            _context = context;
        }

        public async Task Add(Currency currency, CancellationToken cancellationToken)
        {
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(Currency currency, CancellationToken cancellationToken)
        {
            _context.Currencies.Update(currency);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Currency?> GetCurrencyById(Guid currencyId, CancellationToken cancellationToken)
        {
            var category = await _context.Currencies.FirstOrDefaultAsync(c => c.Id == currencyId, cancellationToken);
            return category;
        }
    }
}