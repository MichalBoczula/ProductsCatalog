using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.Common.Filters;

namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories
{
    public interface IMobilePhonesQueriesRepository
    {
        Task<MobilePhoneReadModel?> GetById(Guid id, CancellationToken ct);
        Task<IReadOnlyList<MobilePhoneReadModel>> GetPhones(int amount, CancellationToken ct);
        Task<IReadOnlyList<MobilePhonesHistory>> GetHistoryOfChanges(Guid mobilePhoneId, int pageNumber, int pageSize, CancellationToken ct);
        Task<IReadOnlyList<MobilePhoneReadModel>> GetTop(CancellationToken ct);
        Task<IReadOnlyList<MobilePhoneReadModel>> GetFilteredPhones(MobilePhoneFilterDto mobilePhoneFilter, CancellationToken ct);
    }
}
