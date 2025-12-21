namespace ProductCatalog.Domain.Validation.Common
{
    public class ValidationRuleDescriptor
    {
        public string RuleName { get; set; }
        public List<ValidationError> Rules { get; set; }
    }
}
