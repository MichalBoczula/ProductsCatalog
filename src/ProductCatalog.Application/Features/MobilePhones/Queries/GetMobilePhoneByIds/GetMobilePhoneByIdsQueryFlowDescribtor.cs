using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds
{
    internal sealed class GetMobilePhoneByIdsQueryFlowDescribtor : FlowDescriberBase<GetMobilePhoneByIdsQuery>
    {
        [FlowStep(1)]
        public Task<IReadOnlyList<MobilePhoneReadModel>> GetMobilePhones(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            IReadOnlyCollection<Guid> mobilePhoneIds,
            CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetByIds(mobilePhoneIds, cancellationToken);
        }

        [FlowStep(2)]
        public IReadOnlyList<MobilePhoneReadModel> EnsureAllMobilePhonesFound(
            IReadOnlyList<MobilePhoneReadModel> mobilePhones,
            IReadOnlyCollection<Guid> requestedIds)
        {
            var foundIds = mobilePhones.Select(mobilePhone => mobilePhone.Id).ToHashSet();
            var missingIds = requestedIds.Distinct().Where(id => !foundIds.Contains(id)).ToList().AsReadOnly();

            if (missingIds.Count > 0)
            {
                throw new ResourceNotFoundException(
                    nameof(GetMobilePhoneByIdsQuery),
                    missingIds,
                    nameof(MobilePhoneDto));
            }

            return mobilePhones;
        }

        [FlowStep(3)]
        public IReadOnlyList<MobilePhoneDto> MapMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones.Adapt<List<MobilePhoneDto>>().AsReadOnly();
        }
    }
}
