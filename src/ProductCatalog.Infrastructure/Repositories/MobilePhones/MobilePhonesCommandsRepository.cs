using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Microsoft.EntityFrameworkCore;

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

        public Task SaveChanges(CancellationToken cancellationToken)
            => _db.SaveChangesAsync(cancellationToken);
    }
}
