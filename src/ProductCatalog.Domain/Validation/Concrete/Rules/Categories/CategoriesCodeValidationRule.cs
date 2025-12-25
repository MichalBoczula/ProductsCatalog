using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Categories
{
    public sealed class CategoriesCodeValidationRule : IValidationRule<Category>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public CategoriesCodeValidationRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Code cannot be null or whitespace.",
                Name = nameof(CategoriesCodeValidationRule),
                Entity = nameof(Category)
            };
        }

        public void IsValid(Category entity, ValidationResult validationResults)
        {
            if (entity == null) return;
            if (string.IsNullOrWhiteSpace(entity.Code))
                validationResults.AddValidationError(nullOrWhiteSpace);
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
