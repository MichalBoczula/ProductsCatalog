using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Products.MoneyRule
{
    public sealed class MoneyAmountValidationRule : IValidationRule<Money>
    {
        private readonly ValidationError amountIsZeroOrBelow;

        public MoneyAmountValidationRule()
        {
            amountIsZeroOrBelow = new ValidationError
            {
                Message = "Amount cannot be zero or below.",
                Name =  nameof(MoneyAmountValidationRule),
                Entity = nameof(Money)
            };
        }

        public void IsValid(Money entity, ValidationResult validationResults)
        {
            if (entity.Amount <= 0)
                validationResults.AddValidationError(amountIsZeroOrBelow);
        }

        public List<ValidationError> Describe()
        {
            return [amountIsZeroOrBelow];
        }
    }
}
