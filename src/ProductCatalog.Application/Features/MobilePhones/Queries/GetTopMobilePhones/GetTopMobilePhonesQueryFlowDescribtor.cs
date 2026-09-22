using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones
{
    internal class GetTopMobilePhonesQueryFlowDescribtor : FlowDescriberBase<GetTopMobilePhonesQuery>
    {
        [FlowStep(1)]
        public virtual Task<IReadOnlyList<MobilePhoneReadModel>> GetTopMobilePhones(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetTop(cancellationToken);
        }

        [FlowStep(2)]
        public virtual IReadOnlyList<MobilePhoneReadModel> EnsureTopMobilePhonesFound(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones ?? [];
        }

        [FlowStep(3)]
        public virtual IReadOnlyList<TopMobilePhoneDto> MapTopMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones.Adapt<List<TopMobilePhoneDto>>().AsReadOnly();
        }
    }
}
