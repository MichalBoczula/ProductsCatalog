namespace ProductCatalog.Domain.Validation.Common
{
    public class ValidationResult
    {
        public bool IsValid => !ValidationErrors.Any();
        public List<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();
    }
}
