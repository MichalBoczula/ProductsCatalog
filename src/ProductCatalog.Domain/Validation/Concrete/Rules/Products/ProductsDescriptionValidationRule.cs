using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Products
{
    public sealed class ProductsDescriptionValidationRule : IValidationRule<Product>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public ProductsDescriptionValidationRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Descriptions cannot be null or whitespace.",
                Name = "DescriptionsIsNullOrWhiteSpace",
            };
        }

        public void IsValid(Product entity, ValidationResult validationResults)
        {
            if (entity == null) return;
            if (string.IsNullOrWhiteSpace(entity.Description))
                validationResults.AddValidationError(nullOrWhiteSpace);
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
