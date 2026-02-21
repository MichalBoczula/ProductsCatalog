using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Common
{
    public sealed class AmountGreaterThanZeroValidationRule : IValidationRule<int>
    {
        private readonly ValidationError amountMustBeGreaterThanZero;

        public AmountGreaterThanZeroValidationRule()
        {
            amountMustBeGreaterThanZero = new ValidationError
            {
                Message = "Amount must be greater than zero.",
                Name = nameof(AmountGreaterThanZeroValidationRule),
                Entity = "Amount"
            };
        }

        public Task IsValid(int amount, ValidationResult validationResults)
        {
            if (amount <= 0)
            {
                validationResults.AddValidationError(amountMustBeGreaterThanZero);
            }

            return Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return [amountMustBeGreaterThanZero];
        }
    }
}
