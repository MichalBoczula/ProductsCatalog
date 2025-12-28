using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Abstract
{
    public interface IValidationPolicyDescriptorProvider
    {
        ValidationPolicyDescriptor Describe();
    }
}
