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
                Name = nameof(CurrenciesCodeValidationRule),
                Entity = nameof(Currency)
            };
        }

        public async Task IsValid(Currency entity, ValidationResult validationResults)
        {
            if (entity == null) return;
            if (string.IsNullOrWhiteSpace(entity.Code))
                validationResults.AddValidationError(nullOrWhiteSpace);
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
