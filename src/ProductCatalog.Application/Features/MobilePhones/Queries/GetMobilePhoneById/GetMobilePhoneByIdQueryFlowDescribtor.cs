using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById
{
    [Documentation("products.flow.by-id", "Read current details for an existing ID, including an inactive phone.")]
    [FlowOutcome("products.branch.by-id.found", "The phone exists, active or inactive.", "Return current details as 200; do not write.")]
    [FlowOutcome("products.branch.by-id.missing", "No current phone exists for the supplied ID.", "Return resource_not_found 404 without writes.")]
    [FlowOutcome("products.branch.by-id.repository-failure", "The by-ID repository read throws.", "Return safe internal_error 500; no current/history writes.")]
    internal sealed class GetMobilePhoneByIdQueryFlowDescribtor : FlowDescriberBase<GetMobilePhoneByIdQuery>
    {
        [FlowStep(1, "load", "Query SQL by phone ID without an IsActive filter; cancellation is propagated.")]
        public Task<MobilePhoneReadModel?> GetMobilePhone(IMobilePhonesQueriesRepository mobilePhonesQueriesRepository, Guid mobilePhoneId, CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetById(mobilePhoneId, cancellationToken);
        }

        [FlowStep(2, "require-found", "If SQL returns no phone, throw resource_not_found; a read has no current/history side effects.")]
        public MobilePhoneReadModel EnsureMobilePhoneFound(MobilePhoneReadModel? mobilePhone, Guid mobilePhoneId)
        {
            if (mobilePhone is null)
            {
                throw new ResourceNotFoundException(nameof(GetMobilePhoneByIdQuery), mobilePhoneId, nameof(MobilePhoneDetailsDto));
            }

            return mobilePhone;
        }

        [FlowStep(3, "map-response", "Map full current details, including photo references, to a 200 response.")]
        public MobilePhoneDetailsDto MapMobilePhoneToDto(MobilePhoneReadModel mobilePhone)
        {
            return mobilePhone.Adapt<MobilePhoneDetailsDto>();
        }
    }
}
