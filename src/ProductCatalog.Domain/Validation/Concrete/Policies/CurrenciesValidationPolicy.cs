using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Currencies;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public class CurrenciesValidationPolicy : IValidationPolicy<Currency>, IValidationPolicyDescriptorProvider
    {
        private readonly List<IValidationRule<Currency>> _rules = [];

        public CurrenciesValidationPolicy()
        {
            _rules.Add(new CurrenciesCodeValidationRule());
            _rules.Add(new CurrenciesDescriptionValidationRule());
            _rules.Add(new CurrencyIsNullValidationRule());
        }

        public async Task<ValidationResult> Validate(Currency client)
        {
            ValidationResult validationResult = new();
            _rules.ForEach(rule => rule.IsValid(client, validationResult));
            return validationResult;
        }

        public ValidationPolicyDescriptor Describe()
        {
            var allErrors = _rules
                .Select(rule => new ValidationRuleDescriptor()
                {
                    RuleName = rule.GetType().Name,
                    Rules = rule.Describe()
                })
                .ToList();

            return new ValidationPolicyDescriptor()
            {
                PolicyName = nameof(CurrenciesValidationPolicy),
                Rules = allErrors
            };
        }
    }
}
