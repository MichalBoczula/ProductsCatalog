using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Products.MoneyRule
{
    public sealed class MoneyCurrencyValidationRule : IValidationRule<Money>
    {
        private readonly ValidationError currencyIsNullOrEmpty;
        private readonly ValidationError currencyNotExists;
        private readonly ICurrenciesQueriesRepository _currenciesQueriesRepository;

        public MoneyCurrencyValidationRule(ICurrenciesQueriesRepository currenciesQueriesRepository)
        {
            _currenciesQueriesRepository = currenciesQueriesRepository;
            currencyIsNullOrEmpty = new ValidationError
            {
                Message = "Currency cannot be null or whitespace.",
                Name = nameof(MoneyCurrencyValidationRule),
                Entity = nameof(Money)
            };
            currencyNotExists = new ValidationError
            {
                Message = "Currency does not exist.",
                Name = nameof(MoneyCurrencyValidationRule),
                Entity = nameof(Money)
            };
        }

        public async Task IsValid(Money entity, ValidationResult validationResults)
        {
            if (string.IsNullOrWhiteSpace(entity.Currency))
                validationResults.AddValidationError(currencyIsNullOrEmpty);

            var currency = (await _currenciesQueriesRepository.GetCurrencies(CancellationToken.None))
                .FirstOrDefault(x => x.Code == entity.Currency);

            if (currency is null)
                validationResults.AddValidationError(currencyNotExists);
        }

        public List<ValidationError> Describe()
        {
            return [currencyIsNullOrEmpty, currencyNotExists];
        }
    }
}
