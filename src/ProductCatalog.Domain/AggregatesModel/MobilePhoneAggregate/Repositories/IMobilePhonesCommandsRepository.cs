using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;

namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories
{
    public interface IMobilePhonesCommandsRepository
    {
        void Add(MobilePhone mobilePhone);
        void Update(MobilePhone mobilePhone);
        Task<MobilePhone?> GetById(Guid mobilePhoneId, CancellationToken cancellationToken);
        void WriteHistory(MobilePhonesHistory entity);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
