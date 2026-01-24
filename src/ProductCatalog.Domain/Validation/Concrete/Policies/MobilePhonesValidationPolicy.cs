using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products.MoneyRule;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class MobilePhonesValidationPolicy
        : IValidationPolicy<MobilePhone>, IValidationPolicyDescriptorProvider
    {
        private readonly List<IValidationRule<MobilePhone>> _rules = [];
        private readonly List<IValidationRule<Money>> _moneyRules = [];

        public MobilePhonesValidationPolicy(
            ICategoriesQueriesRepository categoriesQueriesRepository,
            ICurrenciesQueriesRepository currenciesQueriesRepository)
        {
            _rules.Add(new MobilePhonesCommonDescriptionValidationRule());
            _rules.Add(new MobilePhonesDescriptionsValidationRule());
            _rules.Add(new MobilePhonesCategoryIdValidationRule(categoriesQueriesRepository));
            _rules.Add(new MobilePhonesIsNullValidationRule());

            _moneyRules.Add(new MoneyCurrencyValidationRule(currenciesQueriesRepository));
            _moneyRules.Add(new MoneyAmountValidationRule());
        }

        public async Task<ValidationResult> Validate(MobilePhone entity)
        {
            ValidationResult validationResult = new();

            foreach (var rule in _rules)
                await rule.IsValid(entity, validationResult);

            if (entity == null)
                return validationResult;

            foreach (var rule in _moneyRules)
                await rule.IsValid(entity.Price, validationResult);

            return validationResult;
        }

        public ValidationPolicyDescriptor Describe()
        {
            var mobilePhoneErrors = _rules
                .Select(rule => new ValidationRuleDescriptor()
                {
                    RuleName = rule.GetType().Name,
                    Rules = rule.Describe()
                })
                .ToList();

            var moneyErrors = _moneyRules
                .Select(rule => new ValidationRuleDescriptor()
                {
                    RuleName = rule.GetType().Name,
                    Rules = rule.Describe()
                })
                .ToList();

            return new ValidationPolicyDescriptor()
            {
                PolicyName = nameof(MobilePhonesValidationPolicy),
                Rules = mobilePhoneErrors.Concat(moneyErrors).ToList()
            };
        }
    }
}
