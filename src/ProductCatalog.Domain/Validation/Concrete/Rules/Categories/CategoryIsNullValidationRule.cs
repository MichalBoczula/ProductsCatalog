using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Categories
{
    public sealed class CategoryIsNullValidationRule : IValidationRule<Category>
    {
        private readonly ValidationError productIsNull;

        public CategoryIsNullValidationRule()
        {
            productIsNull = new ValidationError
            {
                Message = "Category cannot be null.",
                Name = "CategoryIsNull",
            };
        }

        public void IsValid(Category entity, ValidationResult validationResults)
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
