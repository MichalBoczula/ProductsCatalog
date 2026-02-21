using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Filters;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones
{
    internal class GetFilteredMobilePhonesQueryFlowDescribtor : FlowDescriberBase<GetFilteredMobilePhonesQuery>
    {
        [FlowStep(1)]
        public virtual Task<IReadOnlyList<MobilePhoneReadModel>> GetFilteredMobilePhones(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            MobilePhoneFilterDto mobilePhoneFilter,
            CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetFilteredPhones(mobilePhoneFilter, cancellationToken);
        }

        [FlowStep(2)]
        public virtual IList<MobilePhoneDto> MapMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones.Adapt<IList<MobilePhoneDto>>();
        }
    }
}
