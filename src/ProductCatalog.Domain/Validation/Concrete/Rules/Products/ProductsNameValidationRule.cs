using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Products
{
    public sealed class ProductsNameValidationRule : IValidationRule<Product>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public ProductsNameValidationRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Name cannot be null or whitespace.",
                Name = nameof(ProductsNameValidationRule),
                Entity = nameof(Product)
            };
        }

        public async Task IsValid(Product entity, ValidationResult validationResults)
        {
            if (entity == null) return;
            if (string.IsNullOrWhiteSpace(entity.Name))
                validationResults.AddValidationError(nullOrWhiteSpace);
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
