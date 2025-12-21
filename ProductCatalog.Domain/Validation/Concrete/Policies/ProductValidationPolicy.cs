using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products;
using System.Data;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public class ProductValidationPolicy
    {
        private readonly List<IValidationRule<Product>> _rules = new();

        public ProductValidationPolicy()
        {
            _rules.Add(new ProductsNameValidationPolicy());
        }

        public ValidationResult Validate(Product client)
        {
            var validationResult = new ValidationResult();

            _rules.ForEach(rule=> rule.IsValid(client, validationResult));

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
                PolicyName = nameof(ProductValidationPolicy),
                Rules = allErrors
            };
        }
    }
}
