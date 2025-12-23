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
                RuleName = "CategoryIdIsNullOrWhiteSpace",
            };
        }

        public void IsValid(Product entity, ValidationResult validationResults)
        {
            if (entity.CategoryId == Guid.Empty)
            {
                validationResults.AddValidateError(nullOrEmpty);
                return;
            }
        }

        public List<ValidationError> Describe()
        {
            return [nullOrEmpty];
        }
    }
}
