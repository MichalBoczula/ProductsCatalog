using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones
{
    internal class GetFilteredMobilePhonesQueryFlowDescribtor : FlowDescriberBase<GetFilteredMobilePhonesQuery>
    {
        [FlowStep(1)]
        public virtual Task<ValidationResult> ValidateFilter(MobilePhoneFilterDto mobilePhoneFilter, IValidationPolicy<MobilePhoneFilterDto> validationPolicy)
        {
            return validationPolicy.Validate(mobilePhoneFilter);
        }

        [FlowStep(2)]
        public virtual void ThrowValidationExceptionIfFilterInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(3)]
        public virtual Task<IReadOnlyList<MobilePhoneReadModel>> GetFilteredMobilePhones(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            MobilePhoneFilterDto mobilePhoneFilter,
            CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetFilteredPhones(mobilePhoneFilter, cancellationToken);
        }

        [FlowStep(4)]
        public virtual IList<MobilePhoneDto> MapMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones.Adapt<IList<MobilePhoneDto>>();
        }
    }
}
