using ProductCatalog.Domain.Common.Pagination;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class PaginationParametersValidationPolicy : IValidationPolicy<PaginationParameters>, IValidationPolicyDescriptorProvider
    {
        private readonly List<IValidationRule<PaginationParameters>> _rules = [new PaginationParametersValidationRule()];

        public async Task<ValidationResult> Validate(PaginationParameters pagination)
        {
            ValidationResult result = new();
            foreach (var rule in _rules) await rule.IsValid(pagination, result);
            return result;
        }

        public ValidationPolicyDescriptor Describe() => new()
        {
            PolicyName = nameof(PaginationParametersValidationPolicy),
            Rules = _rules.Select(rule => new ValidationRuleDescriptor { RuleName = rule.GetType().Name, Rules = rule.Describe() }).ToList()
        };
    }
}
