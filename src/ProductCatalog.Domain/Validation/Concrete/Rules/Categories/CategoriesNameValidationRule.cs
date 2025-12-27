using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Categories
{
    public sealed class CategoriesNameValidationRule : IValidationRule<Category>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public CategoriesNameValidationRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Name cannot be null or whitespace.",
                Name = nameof(CategoriesNameValidationRule),
                Entity = nameof(Category)
            };
        }

        public async Task IsValid(Category entity, ValidationResult validationResults)
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
