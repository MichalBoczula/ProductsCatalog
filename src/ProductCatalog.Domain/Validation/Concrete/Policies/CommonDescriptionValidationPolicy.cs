using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class CommonDescriptionValidationPolicy : IValidationPolicy<CommonDescription>, IValidationPolicyDescriptorProvider
    {
        private readonly List<IValidationRule<CommonDescription>> _rules = [];

        public CommonDescriptionValidationPolicy()
        {
            _rules.Add(new CommonDescriptionsNameRule());
            _rules.Add(new CommonDescriptionsDescriptionRule());
            _rules.Add(new CommonDescriptionsMainPhotoRule());
            _rules.Add(new CommonDescriptionsOtherPhotosRule());
        }

        public async Task<ValidationResult> Validate(CommonDescription entity)
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
                PolicyName = nameof(CommonDescriptionValidationPolicy),
                Rules = allErrors
            };
        }
    }
}
