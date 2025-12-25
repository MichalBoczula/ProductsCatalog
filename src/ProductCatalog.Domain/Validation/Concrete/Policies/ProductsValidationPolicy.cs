using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products.MoneyRule;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class ProductsValidationPolicy : IValidationPolicy<Product>
    {
        private readonly List<IValidationRule<Product>> _rules = [];
        private readonly List<IValidationRule<Money>> _moneyRules = [];

        public ProductsValidationPolicy()
        {
            _rules.Add(new ProductsNameValidationRule());
            _rules.Add(new ProductsDescriptionValidationRule());
            _rules.Add(new ProductsCategoryIdValidationRule());
            _rules.Add(new ProductsIsNullValidationRule());

            _moneyRules.Add(new MoneyCurrencyValidationRule());
            _moneyRules.Add(new MoneyAmountValidationRule());
        }

        public ValidationResult Validate(Product entity)
        {
            ValidationResult validationResult = new();
            _rules.ForEach(rule => rule.IsValid(entity, validationResult));
           
            if (entity == null)
                return validationResult;

            _moneyRules.ForEach(rule => rule.IsValid(entity.Price, validationResult));
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

            var moneyErrors = _moneyRules
                .Select(rule => new ValidationRuleDescriptor()
                {
                    RuleName = rule.GetType().Name,
                    Rules = rule.Describe()
                })
                .ToList();

            return new ValidationPolicyDescriptor()
            {
                PolicyName = nameof(ProductsValidationPolicy),
                Rules = allErrors.Concat(moneyErrors).ToList()
            };
        }
    }
}
