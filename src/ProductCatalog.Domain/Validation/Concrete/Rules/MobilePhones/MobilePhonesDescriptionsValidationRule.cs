using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones
{
    public sealed class MobilePhonesDescriptionsValidationRule : IValidationRule<MobilePhone>
    {
        private readonly ValidationError description2IsNullOrWhitespace;
        private readonly ValidationError description3IsNullOrWhitespace;

        public MobilePhonesDescriptionsValidationRule()
        {
            description2IsNullOrWhitespace = new ValidationError
            {
                Message = "Description2 cannot be null or whitespace.",
                Name = nameof(MobilePhonesDescriptionsValidationRule),
                Entity = nameof(MobilePhone)
            };
            description3IsNullOrWhitespace = new ValidationError
            {
                Message = "Description3 cannot be null or whitespace.",
                Name = nameof(MobilePhonesDescriptionsValidationRule),
                Entity = nameof(MobilePhone)
            };
        }

        public async Task IsValid(MobilePhone entity, ValidationResult validationResults)
        {
            if (entity == null) return;

            if (string.IsNullOrWhiteSpace(entity.Description2))
                validationResults.AddValidationError(description2IsNullOrWhitespace);

            if (string.IsNullOrWhiteSpace(entity.Description3))
                validationResults.AddValidationError(description3IsNullOrWhitespace);
        }

        public List<ValidationError> Describe()
        {
            return [description2IsNullOrWhitespace, description3IsNullOrWhitespace];
        }
    }
}
