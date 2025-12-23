using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Abstract
{
    public interface IValidationPolicy<T>
    {
        ValidationResult Validate(T entity);
        ValidationPolicyDescriptor Describe();
    }
}
