namespace ProductCatalog.Domain.Validation.Common
{
    public sealed class ValidationResult
    {
        public bool IsValid => !_validationErrors.Any();
        private readonly List<ValidationError> _validationErrors = new();

        public void AddValidationError(ValidationError validationError)
        {
            _validationErrors.Add(validationError);
        }

        public IReadOnlyList<ValidationError> GetValidatonErrors() => _validationErrors.AsReadOnly();
    }
}
