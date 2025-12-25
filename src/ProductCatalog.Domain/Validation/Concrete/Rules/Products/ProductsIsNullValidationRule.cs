using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Products
{
    public sealed class ProductsIsNullValidationRule : IValidationRule<Product>
    {
        private readonly ValidationError productIsNull;

        public ProductsIsNullValidationRule()
        {
            productIsNull = new ValidationError
            {
                Message = "Product cannot be null.",
                Name = nameof(ProductsIsNullValidationRule),
                Entity = nameof(Product),
            };
        }

        public void IsValid(Product entity, ValidationResult validationResults)
        {
            if (entity == null)
                validationResults.AddValidationError(productIsNull);
        }

        public List<ValidationError> Describe()
        {
            return [productIsNull];
        }
    }
}
