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
                Message = "Products name cannot be null or whitespace.",
                RuleName = "ProductsNameIsNullOrWhiteSpace",
            };
        }

        public void IsValid(Product entity, ValidationResult validationResults)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                validationResults.AddValidateError(nullOrWhiteSpace);
                return;
            }
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
