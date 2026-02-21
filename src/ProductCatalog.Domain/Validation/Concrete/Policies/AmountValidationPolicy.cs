using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class AmountValidationPolicy : IValidationPolicy<int>, IValidationPolicyDescriptorProvider
    {
        private readonly List<IValidationRule<int>> _rules = [];

        public AmountValidationPolicy()
        {
            _rules.Add(new AmountGreaterThanZeroValidationRule());
        }

        public Task<ValidationResult> Validate(int amount)
        {
            ValidationResult validationResult = new();
            _rules.ForEach(rule => rule.IsValid(amount, validationResult));
            return Task.FromResult(validationResult);
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
                PolicyName = nameof(AmountValidationPolicy),
                Rules = allErrors
            };
        }
    }
}
