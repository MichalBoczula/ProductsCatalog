using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.ElectronicDetails;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class ElectronicDetailsValidationPolicy
        : IValidationPolicy<ElectronicDetails>, IValidationPolicyDescriptorProvider
    {
        private readonly List<IValidationRule<ElectronicDetails>> _rules = [];

        public ElectronicDetailsValidationPolicy()
        {
            _rules.Add(new ElectronicDetailsStringsValidationRule());
            _rules.Add(new ElectronicDetailsNumbersValidationRule());
        }

        public async Task<ValidationResult> Validate(ElectronicDetails entity)
        {
            ValidationResult validationResult = new();

            foreach (var rule in _rules)
                await rule.IsValid(entity, validationResult);

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
                PolicyName = nameof(ElectronicDetailsValidationPolicy),
                Rules = allErrors
            };
        }
    }
}
