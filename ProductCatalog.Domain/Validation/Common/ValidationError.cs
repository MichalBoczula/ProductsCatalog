using ProductCatalog.Domain.Validation.Enums;

namespace ProductCatalog.Domain.Validation.Common
{
    public class ValidationError
    {
        public string Message { get; set; }
        public string RuleName { get; set; }
        public RuleSeverity Severity { get; set; }
    }
}
