namespace ProductCatalog.Domain.Validation.Common
{
    public class ValidationPolicyDescriptor
    {
        public string PolicyName { get; set; }
        public List<ValidationRuleDescriptor> Rules { get; set; }
    }
}
