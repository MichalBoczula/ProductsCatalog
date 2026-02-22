using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones
{
    public sealed class MobilePhoneFilterPriceRangeValidationRule : IValidationRule<MobilePhoneFilterDto>
    {
        private readonly ValidationError minimalPriceMustBeLowerThanMaximalPrice;

        public MobilePhoneFilterPriceRangeValidationRule()
        {
            minimalPriceMustBeLowerThanMaximalPrice = new ValidationError
            {
                Message = "Minimal price must be lower than maximal price.",
                Name = nameof(MobilePhoneFilterPriceRangeValidationRule),
                Entity = nameof(MobilePhoneFilterDto)
            };
        }

        public Task IsValid(MobilePhoneFilterDto mobilePhoneFilter, ValidationResult validationResult)
        {
            if (mobilePhoneFilter is null)
            {
                return Task.CompletedTask;
            }

            if (mobilePhoneFilter.MinimalPrice >= mobilePhoneFilter.MaximalPrice)
            {
                validationResult.AddValidationError(minimalPriceMustBeLowerThanMaximalPrice);
            }

            return Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return [minimalPriceMustBeLowerThanMaximalPrice];
        }
    }
}
