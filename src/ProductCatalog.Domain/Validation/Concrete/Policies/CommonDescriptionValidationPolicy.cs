using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Policies
{
    public sealed class CommonDescriptionValidationPolicy : IValidationPolicy<CommonDescription>, IValidationPolicyDescriptorProvider
    {
        public Task<ValidationResult> Validate(CommonDescription entity)
        {
            throw new NotImplementedException();
        }

        public ValidationPolicyDescriptor Describe()
        {
            throw new NotImplementedException();
        }
    }
}
