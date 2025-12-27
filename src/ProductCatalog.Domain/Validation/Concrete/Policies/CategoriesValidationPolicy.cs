using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Categories;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class CategoriesValidationPolicy : IValidationPolicy<Category>
    {
        private readonly List<IValidationRule<Category>> _rules = [];

        public CategoriesValidationPolicy()
        {
            _rules.Add(new CategoriesNameValidationRule());
            _rules.Add(new CategoriesCodeValidationRule());
            _rules.Add(new CategoryIsNullValidationRule());
        }

        public async Task<ValidationResult> Validate(Category client)
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
                PolicyName = nameof(CategoriesValidationPolicy),
                Rules = allErrors
            };
        }
    }
}
