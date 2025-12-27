using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Products
{
    public sealed class ProductsCategoryIdValidationRule
        : IValidationRule<Product>
    {
        private readonly ICategoriesQueriesRepository _categoriesQueriesRepository;
        private readonly ValidationError nullOrEmpty;
        private readonly ValidationError categoryNotExists;

        public ProductsCategoryIdValidationRule(ICategoriesQueriesRepository categoriesQueriesRepository)
        {
            nullOrEmpty = new ValidationError
            {
                Message = "CategoryId cannot be null or empty.",
                Name = nameof(ProductsCategoryIdValidationRule),
                Entity = nameof(Product)
            };
            categoryNotExists = new ValidationError
            {
                Message = "CategoryId does not exist.",
                Name = nameof(ProductsCategoryIdValidationRule),
                Entity = nameof(Product)
            };
            _categoriesQueriesRepository = categoriesQueriesRepository;
        }

        public async Task IsValid(Product entity, ValidationResult validationResults)
        {
            if (entity == null) return;
            if (entity.CategoryId == Guid.Empty)
                validationResults.AddValidationError(nullOrEmpty);
            
            var category = await _categoriesQueriesRepository.GetById(entity.CategoryId, CancellationToken.None);
            if (category == null)
                validationResults.AddValidationError(categoryNotExists);
        }

        public List<ValidationError> Describe()
        {
            return [nullOrEmpty, categoryNotExists];
        }
    }
}
