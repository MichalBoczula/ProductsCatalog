using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Currencies
{
    public sealed class CurrencyIsNullValidationRule : IValidationRule<Currency>
    {
        private readonly ValidationError currencyIsNull;

        public CurrencyIsNullValidationRule()
        {
            currencyIsNull = new ValidationError
            {
                Message = "Currency cannot be null.",
                Name = nameof(CurrencyIsNullValidationRule),
                Entity = nameof(Currency)
            };
        }

        public void IsValid(Currency entity, ValidationResult validationResults)
        {
            if (entity == null)
                validationResults.AddValidationError(currencyIsNull);
        }

        public List<ValidationError> Describe()
        {
            return [currencyIsNull];
        }
    }
}
