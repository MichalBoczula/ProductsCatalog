using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Infrastructure.Repositories.MobilePhones
{
    internal sealed class MobilePhonesCommandsRepository : IMobilePhonesCommandsRepository
    {
        private readonly ProductsContext _db;

        public MobilePhonesCommandsRepository(ProductsContext db)
        {
            _db = db;
        }

        public void Add(MobilePhone mobilePhone)
        {
            _db.MobilePhones.Add(mobilePhone);
        }

        public void Update(MobilePhone mobilePhone)
        {
            _db.MobilePhones.Update(mobilePhone);
        }

        public Task<MobilePhone?> GetById(Guid mobilePhoneId, CancellationToken cancellationToken)
        {
            return _db.MobilePhones.FirstOrDefaultAsync(x => x.Id == mobilePhoneId, cancellationToken);
        }

        public void WriteHistory(MobilePhonesHistory entity)
        {
            _db.MobilePhonesHistories.Add(entity);
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            try
            {
                // EF Core saves the phone and its history in the same SaveChanges transaction.
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException exception) when (exception.Entries.Any(x => x.Entity is MobilePhone))
            {
                var phoneId = ((MobilePhone)exception.Entries.First(x => x.Entity is MobilePhone).Entity).Id;
                _db.ChangeTracker.Clear();
                var exists = await _db.MobilePhones.AsNoTracking()
                    .AnyAsync(x => x.Id == phoneId, cancellationToken);
                if (!exists)
                    throw new ResourceNotFoundException(nameof(SaveChanges), phoneId, nameof(MobilePhone));

                throw new ConcurrencyConflictException();
            }
        }
    }
}
