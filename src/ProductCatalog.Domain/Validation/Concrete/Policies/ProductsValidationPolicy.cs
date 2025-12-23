using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class ProductsValidationPolicy : IValidationPolicy<Product>
    {
        private readonly List<IValidationRule<Product>> _rules = [];
        private readonly ValidationResult validationResult = new();

        public ProductsValidationPolicy()
        {
            _rules.Add(new ProductsNameValidationRule());
            _rules.Add(new ProductsDescriptionValidationRule());
            _rules.Add(new ProductsCategoryIdValidationRule());
            _rules.Add(new ProductsIsNullValidationRule());
        }

        public ValidationResult Validate(Product client)
        {
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
                PolicyName = nameof(ProductsValidationPolicy),
                Rules = allErrors
            };
        }
    }
}
