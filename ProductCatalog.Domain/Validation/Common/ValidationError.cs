using ProductCatalog.Domain.Validation.Enums;

namespace ProductCatalog.Domain.Validation.Common
{
    public class ValidationError
    {
        public required string Message { get; init; }
        public required string RuleName { get; init; }
        public required RuleSeverity Severity { get; init; }
    }
}
