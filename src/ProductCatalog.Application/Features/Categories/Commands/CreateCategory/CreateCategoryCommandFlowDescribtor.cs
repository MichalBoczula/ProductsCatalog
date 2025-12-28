using Mapster;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Categories.Commands.CreateCategory
{
    internal sealed class CreateCategoryCommandFlowDescribtor : FlowDescriberBase<CreateCategoryCommand>
    {
        [FlowStep(1)]
        public Category MapRequestToCategoryAggregate(CreateCategoryCommand command)
        {
            var category = command.category.Adapt<Category>();
            return category;
        }

        [FlowStep(2)]
        public async Task<ValidationResult> ValidateCategoryAggregate(Category category, IValidationPolicy<Category> validationPolicy)
        {
            var validationResult = await validationPolicy.Validate(category);
            return validationResult;
        }

        [FlowStep(3)]
        public void ThrowValidationExceptionIfNotValid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4)]
        public void AddCategoryToRepository(Category category, ICategoriesCommandsRepository categoriesCommandsRepository)
        {
            categoriesCommandsRepository.Add(category);
        }

        [FlowStep(5)]
        public CategoriesHistory CreateCategoryHistoryEntry(Category category)
        {
            var categoriesHistory = category.BuildAdapter()
               .AddParameters("operation", Operation.Inserted)
               .AdaptToType<CategoriesHistory>();

            return categoriesHistory;
        }

        [FlowStep(6)]
        public void WriteHistoryToRepository(ICategoriesCommandsRepository categoriesCommandsRepository, CategoriesHistory categoriesHistory)
        {
            categoriesCommandsRepository.WriteHistory(categoriesHistory);
        }

        [FlowStep(7)]
        public async Task SaveChanges(ICategoriesCommandsRepository categoriesCommandsRepository, CancellationToken cancellationToken)
        {
            await categoriesCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(8)]
        public CategoryDto MapCategoryToCategoryDto(Category category)
        {
            var categoryDto = category.Adapt<CategoryDto>();
            return categoryDto;
        }
    }
}
