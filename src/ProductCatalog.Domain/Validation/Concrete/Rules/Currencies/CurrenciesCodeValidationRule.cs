using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Currencies
{
    public sealed class CurrenciesCodeValidationRule : IValidationRule<Currency>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public CurrenciesCodeValidationRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Code cannot be null or whitespace.",
                Name = "CodeIsNullOrWhiteSpace",
            };
        }

        public void IsValid(Currency entity, ValidationResult validationResults)
        {
            if (entity == null) return;
            if (string.IsNullOrWhiteSpace(entity.Description))
                validationResults.AddValidationError(nullOrWhiteSpace);
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
