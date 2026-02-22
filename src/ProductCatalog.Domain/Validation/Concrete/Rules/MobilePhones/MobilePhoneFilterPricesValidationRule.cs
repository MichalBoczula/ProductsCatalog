using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones
{
    public sealed class MobilePhoneFilterPricesValidationRule : IValidationRule<MobilePhoneFilterDto>
    {
        private readonly ValidationError minimalPriceMustBeGreaterThanOrEqualToZero;
        private readonly ValidationError maximalPriceMustBeGreaterThanOrEqualToZero;

        public MobilePhoneFilterPricesValidationRule()
        {
            minimalPriceMustBeGreaterThanOrEqualToZero = new ValidationError
            {
                Message = "Minimal price must be greater than or equal to zero.",
                Name = nameof(MobilePhoneFilterPricesValidationRule),
                Entity = nameof(MobilePhoneFilterDto)
            };

            maximalPriceMustBeGreaterThanOrEqualToZero = new ValidationError
            {
                Message = "Maximal price must be greater than or equal to zero.",
                Name = nameof(MobilePhoneFilterPricesValidationRule),
                Entity = nameof(MobilePhoneFilterDto)
            };
        }

        public Task IsValid(MobilePhoneFilterDto mobilePhoneFilter, ValidationResult validationResult)
        {
            if (mobilePhoneFilter is null)
            {
                return Task.CompletedTask;
            }

            if (mobilePhoneFilter.MinimalPrice < 0)
            {
                validationResult.AddValidationError(minimalPriceMustBeGreaterThanOrEqualToZero);
            }

            if (mobilePhoneFilter.MaximalPrice < 0)
            {
                validationResult.AddValidationError(maximalPriceMustBeGreaterThanOrEqualToZero);
            }

            return Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return [minimalPriceMustBeGreaterThanOrEqualToZero, maximalPriceMustBeGreaterThanOrEqualToZero];
        }
    }
}
