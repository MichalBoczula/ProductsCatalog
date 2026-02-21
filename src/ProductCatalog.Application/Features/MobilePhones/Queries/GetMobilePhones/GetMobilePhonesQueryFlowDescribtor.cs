using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones
{
    internal sealed class GetMobilePhonesQueryFlowDescribtor : FlowDescriberBase<GetMobilePhonesQuery>
    {
        [FlowStep(1)]
        public Task<ValidationResult> ValidateAmount(int amount, IValidationPolicy<int> validationPolicy)
        {
            return validationPolicy.Validate(amount);
        }

        [FlowStep(2)]
        public void ThrowValidationExceptionIfAmountInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(3)]
        public Task<IReadOnlyList<MobilePhoneReadModel>> GetMobilePhones(IMobilePhonesQueriesRepository mobilePhonesQueriesRepository, int amount, CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetPhones(amount, cancellationToken);
        }

        [FlowStep(4)]
        public IReadOnlyList<MobilePhoneReadModel> EnsureMobilePhonesFound(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            if (mobilePhones is null)
            {
                throw new ResourceNotFoundException(nameof(GetMobilePhonesQuery), Guid.Empty, nameof(List<MobilePhoneDetailsDto>));
            }

            return mobilePhones;
        }

        [FlowStep(5)]
        public IReadOnlyList<MobilePhoneDto> MapMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones.Adapt<List<MobilePhoneDto>>().AsReadOnly();
        }
    }
}
