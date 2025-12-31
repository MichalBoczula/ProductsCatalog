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

namespace ProductCatalog.Application.Features.Categories.Commands.DeleteCategory
{
    internal sealed class DeleteCategoryCommandFlowDescribtor : FlowDescriberBase<DeleteCategoryCommand>
    {
        [FlowStep(1)]
        public async Task<Category> LoadCategory(Guid categoryId, ICategoriesCommandsRepository categoriesCommandsRepository, CancellationToken cancellationToken)
        {
            return await categoriesCommandsRepository.GetCategoryById(categoryId, cancellationToken);
        }

        [FlowStep(2)]
        public async Task<ValidationResult> ValidateCategory(Category category, IValidationPolicy<Category> validationPolicy)
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
        public void DeactivateCategory(Category category)
        {
            category.Deactivate();
        }

        [FlowStep(5)]
        public void UpdateCategoryInRepository(Category category, ICategoriesCommandsRepository categoriesCommandsRepository)
        {
            categoriesCommandsRepository.Update(category);
        }

        [FlowStep(6)]
        public CategoriesHistory CreateCategoryHistoryEntry(Category category)
        {
            var categoriesHistory = category.BuildAdapter()
               .AddParameters("operation", Operation.Deleted)
               .AdaptToType<CategoriesHistory>();

            return categoriesHistory;
        }

        [FlowStep(7)]
        public void WriteHistoryToRepository(ICategoriesCommandsRepository categoriesCommandsRepository, CategoriesHistory categoriesHistory)
        {
            categoriesCommandsRepository.WriteHistory(categoriesHistory);
        }

        [FlowStep(8)]
        public async Task SaveChanges(ICategoriesCommandsRepository categoriesCommandsRepository, CancellationToken cancellationToken)
        {
            await categoriesCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(9)]
        public CategoryDto MapCategoryToCategoryDto(Category category)
        {
            var categoryDto = category.Adapt<CategoryDto>();
            return categoryDto;
        }
    }
}
