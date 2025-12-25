using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class CategoriesValidationPolicy : IValidationPolicy<Category>
    {
        private readonly List<IValidationRule<Category>> _rules = [];
        private readonly ValidationResult validationResult = new();

        public CategoriesValidationPolicy()
        {
            
        }

        public ValidationResult Validate(Category client)
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
