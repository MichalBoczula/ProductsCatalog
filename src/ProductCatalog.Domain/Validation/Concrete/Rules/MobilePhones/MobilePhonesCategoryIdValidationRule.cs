using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones
{
    public sealed class MobilePhonesCategoryIdValidationRule : IValidationRule<MobilePhone>
    {
        private readonly ICategoriesQueriesRepository _categoriesQueriesRepository;
        private readonly ValidationError nullOrEmpty;
        private readonly ValidationError categoryNotExists;

        public MobilePhonesCategoryIdValidationRule(ICategoriesQueriesRepository categoriesQueriesRepository)
        {
            _categoriesQueriesRepository = categoriesQueriesRepository;
            nullOrEmpty = new ValidationError
            {
                Message = "CategoryId cannot be null or empty.",
                Name = nameof(MobilePhonesCategoryIdValidationRule),
                Entity = nameof(MobilePhone)
            };
            categoryNotExists = new ValidationError
            {
                Message = "CategoryId does not exist.",
                Name = nameof(MobilePhonesCategoryIdValidationRule),
                Entity = nameof(MobilePhone)
            };
        }

        public async Task IsValid(MobilePhone entity, ValidationResult validationResults)
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
