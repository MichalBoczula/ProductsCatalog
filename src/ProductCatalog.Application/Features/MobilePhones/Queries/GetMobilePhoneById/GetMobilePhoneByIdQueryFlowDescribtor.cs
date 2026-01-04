using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById
{
    internal sealed class GetMobilePhoneByIdQueryFlowDescribtor : FlowDescriberBase<GetMobilePhoneByIdQuery>
    {
        [FlowStep(1)]
        public Task<MobilePhoneReadModel?> GetMobilePhone(IMobilePhonesQueriesRepository mobilePhonesQueriesRepository, Guid mobilePhoneId, CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetById(mobilePhoneId, cancellationToken);
        }

        [FlowStep(2)]
        public MobilePhoneReadModel EnsureMobilePhoneFound(MobilePhoneReadModel? mobilePhone, Guid mobilePhoneId)
        {
            if (mobilePhone is null)
            {
                throw new ResourceNotFoundException(nameof(GetMobilePhoneByIdQuery), mobilePhoneId, nameof(MobilePhoneDto));
            }

            return mobilePhone;
        }

        [FlowStep(3)]
        public MobilePhoneDto MapMobilePhoneToDto(MobilePhoneReadModel mobilePhone)
        {
            return mobilePhone.Adapt<MobilePhoneDto>();
        }
    }
}
