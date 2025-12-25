using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Currencies
{
    public sealed class CurrenciesDescriptionValidationRule : IValidationRule<Currency>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public CurrenciesDescriptionValidationRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Description cannot be null or whitespace.",
                Name = nameof(CurrenciesDescriptionValidationRule),
                Entity = nameof(Currency)
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
