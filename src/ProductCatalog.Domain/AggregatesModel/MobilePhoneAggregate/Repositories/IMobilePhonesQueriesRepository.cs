using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;

namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories
{
    public interface IMobilePhonesQueriesRepository
    {
        Task<MobilePhoneReadModel?> GetById(Guid id, CancellationToken ct);
        Task<IReadOnlyList<MobilePhoneReadModel>> GetPhones(int amount, CancellationToken ct);
        Task<IReadOnlyList<MobilePhoneReadModel>> GetByFilter(CancellationToken ct);
        Task<IReadOnlyList<MobilePhonesHistory>> GetHistoryOfChanges(CancellationToken ct);
    }
}
