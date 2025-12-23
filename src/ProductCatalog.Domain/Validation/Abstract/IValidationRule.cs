using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Abstract
{
    public interface IValidationRule<T>
    {
        void IsValid(T entity, ValidationResult validationResults);

        List<ValidationError> Describe();
    }
}
