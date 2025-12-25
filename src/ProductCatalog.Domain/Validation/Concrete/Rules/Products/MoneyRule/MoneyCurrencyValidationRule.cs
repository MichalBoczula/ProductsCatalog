using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Products.MoneyRule
{
    public sealed class MoneyCurrencyValidationRule : IValidationRule<Money>
    {
        private readonly ValidationError currencyIsNullOrEmpty;

        public MoneyCurrencyValidationRule()
        {
            currencyIsNullOrEmpty = new ValidationError
            {
                Message = "Currency cannot be null or whitespace.",
                Name = nameof(MoneyCurrencyValidationRule),
                Entity = nameof(Money)
            };
        }

        public void IsValid(Money entity, ValidationResult validationResults)
        {
            if (string.IsNullOrWhiteSpace(entity.Currency))
                validationResults.AddValidationError(currencyIsNullOrEmpty);
        }

        public List<ValidationError> Describe()
        {
            return [currencyIsNullOrEmpty];
        }
    }
}
