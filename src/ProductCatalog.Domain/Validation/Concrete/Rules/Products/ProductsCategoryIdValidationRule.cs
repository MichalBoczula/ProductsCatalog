using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Products
{
    public sealed class ProductsCategoryIdValidationRule : IValidationRule<Product>
    {
        private readonly ValidationError nullOrEmpty;

        public ProductsCategoryIdValidationRule()
        {
            nullOrEmpty = new ValidationError
            {
                Message = "CategoryId cannot be null or empty.",
                Name = nameof(ProductsCategoryIdValidationRule),
                Entity = nameof(Product)
            };
        }

        public void IsValid(Product entity, ValidationResult validationResults)
        {
            if (entity == null) return;
            if (entity.CategoryId == Guid.Empty)
                validationResults.AddValidationError(nullOrEmpty);
        }

        public List<ValidationError> Describe()
        {
            return [nullOrEmpty];
        }
    }
}
