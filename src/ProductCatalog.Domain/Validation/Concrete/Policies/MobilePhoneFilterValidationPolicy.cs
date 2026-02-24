using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class MobilePhoneFilterValidationPolicy : IValidationPolicy<MobilePhoneFilterDto>, IValidationPolicyDescriptorProvider
    {
        private readonly List<IValidationRule<MobilePhoneFilterDto>> _rules = [];

        public MobilePhoneFilterValidationPolicy()
        {
            _rules.Add(new MobilePhoneFilterPricesValidationRule());
            _rules.Add(new MobilePhoneFilterPriceRangeValidationRule());
            _rules.Add(new MobilePhoneFilterBrandValidationRule());
        }

        public async Task<ValidationResult> Validate(MobilePhoneFilterDto mobilePhoneFilter)
        {
            ValidationResult validationResult = new();

            foreach (var rule in _rules)
            {
                await rule.IsValid(mobilePhoneFilter, validationResult);
            }

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
                PolicyName = nameof(MobilePhoneFilterValidationPolicy),
                Rules = allErrors
            };
        }
    }
}
