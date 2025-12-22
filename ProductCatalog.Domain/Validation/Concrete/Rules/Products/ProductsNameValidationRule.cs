using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Enums;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Products
{
    public class ProductsNameValidationRule : IValidationRule<Product>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public ProductsNameValidationRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Products name cannot be null or whitespace.",
                RuleName = "ProductsNameIsNullOrWhiteSpace",
                Severity = RuleSeverity.Error
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
            return new List<ValidationError> { nullOrWhiteSpace };
        }
    }
}
