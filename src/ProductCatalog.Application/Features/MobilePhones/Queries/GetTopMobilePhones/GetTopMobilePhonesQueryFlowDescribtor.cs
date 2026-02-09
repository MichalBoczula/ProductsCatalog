using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones
{
    internal class GetTopMobilePhonesQueryFlowDescribtor : FlowDescriberBase<GetTopMobilePhonesQuery>
    {
        [FlowStep(1)]
        public virtual Task<IReadOnlyList<MobilePhoneReadModel>> GetTopMobilePhones(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            int amount,
            CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetPhones(amount, cancellationToken);
        }

        [FlowStep(2)]
        public virtual IReadOnlyList<MobilePhoneReadModel> EnsureTopMobilePhonesFound(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            if (mobilePhones is null)
            {
                throw new ResourceNotFoundException(nameof(GetTopMobilePhonesQuery), Guid.Empty, nameof(List<TopMobilePhoneDto>));
            }

            return mobilePhones;
        }

        [FlowStep(3)]
        public virtual IReadOnlyList<TopMobilePhoneDto> MapTopMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
    {
            return mobilePhones.Adapt<List<TopMobilePhoneDto>>().AsReadOnly();
        }
    }
}
