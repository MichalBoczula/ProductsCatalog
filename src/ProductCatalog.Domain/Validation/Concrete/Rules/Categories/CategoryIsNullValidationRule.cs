using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Categories
{
    public sealed class CategoryIsNullValidationRule : IValidationRule<Category>
    {
        private readonly ValidationError categoryIsNull;

        public CategoryIsNullValidationRule()
        {
            categoryIsNull = new ValidationError
            {
                Message = "Category cannot be null.",
                Name = nameof(CategoryIsNullValidationRule),
                Entity = nameof(Category)
            };
        }

        public void IsValid(Category entity, ValidationResult validationResults)
        {
            if (entity == null)
                validationResults.AddValidationError(categoryIsNull);
        }

        public List<ValidationError> Describe()
        {
            return [categoryIsNull];
        }
    }
}
