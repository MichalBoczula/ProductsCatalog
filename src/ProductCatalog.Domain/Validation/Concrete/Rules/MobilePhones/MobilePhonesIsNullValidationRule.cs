using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones
{
    public sealed class MobilePhonesIsNullValidationRule : IValidationRule<MobilePhone>
    {
        private readonly ValidationError mobilePhoneIsNull;

        public MobilePhonesIsNullValidationRule()
        {
            mobilePhoneIsNull = new ValidationError
            {
                Message = "Mobile phone cannot be null.",
                Name = nameof(MobilePhonesIsNullValidationRule),
                Entity = nameof(MobilePhone),
            };
        }

        public async Task IsValid(MobilePhone entity, ValidationResult validationResults)
        {
            if (entity == null)
                validationResults.AddValidationError(mobilePhoneIsNull);
        }

        public List<ValidationError> Describe()
        {
            return [mobilePhoneIsNull];
        }
    }
}
