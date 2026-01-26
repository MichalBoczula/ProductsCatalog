using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones
{
    public sealed class MobilePhonesCommonDescriptionValidationRule : IValidationRule<MobilePhone>
    {
        private readonly ValidationError commonDescriptionIsNull;
        private readonly ValidationError nameIsNullOrWhitespace;
        private readonly ValidationError descriptionIsNullOrWhitespace;
        private readonly ValidationError brandIsNullOrWhitespace;

        public MobilePhonesCommonDescriptionValidationRule()
        {
            commonDescriptionIsNull = new ValidationError
            {
                Message = "Common description cannot be null.",
                Name = nameof(MobilePhonesCommonDescriptionValidationRule),
                Entity = nameof(MobilePhone)
            };
            nameIsNullOrWhitespace = new ValidationError
            {
                Message = "Name cannot be null or whitespace.",
                Name = nameof(MobilePhonesCommonDescriptionValidationRule),
                Entity = nameof(MobilePhone)
            };
            descriptionIsNullOrWhitespace = new ValidationError
            {
                Message = "Description cannot be null or whitespace.",
                Name = nameof(MobilePhonesCommonDescriptionValidationRule),
                Entity = nameof(MobilePhone)
            };
            brandIsNullOrWhitespace = new ValidationError
            {
                Message = "Brand cannot be null or whitespace.",
                Name = nameof(MobilePhonesCommonDescriptionValidationRule),
                Entity = nameof(MobilePhone)
            };
        }

        public async Task IsValid(MobilePhone entity, ValidationResult validationResults)
        {
            if (entity == null) return;

            if (entity.CommonDescription == default)
            {
                validationResults.AddValidationError(commonDescriptionIsNull);
                return;
            }

            if (string.IsNullOrWhiteSpace(entity.CommonDescription.Name))
                validationResults.AddValidationError(nameIsNullOrWhitespace);

            if (string.IsNullOrWhiteSpace(entity.CommonDescription.Description))
                validationResults.AddValidationError(descriptionIsNullOrWhitespace);

            if (string.IsNullOrWhiteSpace(entity.CommonDescription.Brand))
                validationResults.AddValidationError(brandIsNullOrWhitespace);
        }

        public List<ValidationError> Describe()
        {
            return [commonDescriptionIsNull, nameIsNullOrWhitespace, descriptionIsNullOrWhitespace, brandIsNullOrWhitespace];
        }
    }
}
