namespace ProductCatalog.Domain.Validation.Common
{
    public sealed class ValidationResult
    {
        public bool IsValid => !_validationErrors.Any();
        private readonly List<ValidationError> _validationErrors = new();

        public void AddValidateError(ValidationError validationError)
        {
            _validationErrors.Add(validationError);
        }

        public IReadOnlyList<ValidationError> GetValidateErrors() => _validationErrors.AsReadOnly();
    }
}
