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

namespace ProductCatalog.Application.Features.Categories.Commands.UpdateCategory
{
    internal sealed class UpdateCategoryCommandFlowDescribtor : FlowDescriberBase<UpdateCategoryCommand>
    {
        [FlowStep(1)]
        public Category MapRequestToCategoryAggregate(UpdateCategoryCommand command)
        {
            var incoming = command.category.Adapt<Category>();
            return incoming;
        }

        [FlowStep(2)]
        public Task<ValidationResult> ValidateIncomingCategory(Category category, IValidationPolicy<Category> validationPolicy)
        {
            var validationResult = validationPolicy.Validate(category);
            return validationResult;
        }

        [FlowStep(3)]
        public void ThrowValidationExceptionIfIncomingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4)]
        public Task<Category> LoadExistingCategory(Guid categoryId, ICategoriesCommandsRepository categoriesCommandsRepository, CancellationToken cancellationToken)
        {
            return categoriesCommandsRepository.GetCategoryById(categoryId, cancellationToken);
        }

        [FlowStep(5)]
        public Task<ValidationResult> ValidateExistingCategory(Category category, IValidationPolicy<Category> validationPolicy)
        {
            var validationResult = validationPolicy.Validate(category);
            return validationResult;
        }

        [FlowStep(6)]
        public void ThrowValidationExceptionIfExistingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(7)]
        public void AssignNewCategoryInformation(Category existingCategory, Category incomingCategory)
        {
            existingCategory.AssigneNewCategoryInformation(incomingCategory);
        }

        [FlowStep(8)]
        public void UpdateCategoryInRepository(Category category, ICategoriesCommandsRepository categoriesCommandsRepository)
        {
            categoriesCommandsRepository.Update(category);
        }

        [FlowStep(9)]
        public CategoriesHistory CreateCategoryHistoryEntry(Category category)
        {
            var categoriesHistory = category.BuildAdapter()
               .AddParameters("operation", Operation.Updated)
               .AdaptToType<CategoriesHistory>();

            return categoriesHistory;
        }

        [FlowStep(10)]
        public void WriteHistoryToRepository(ICategoriesCommandsRepository categoriesCommandsRepository, CategoriesHistory categoriesHistory)
        {
            categoriesCommandsRepository.WriteHistory(categoriesHistory);
        }

        [FlowStep(11)]
        public Task SaveChanges(ICategoriesCommandsRepository categoriesCommandsRepository, CancellationToken cancellationToken)
        {
            return categoriesCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(12)]
        public CategoryDto MapCategoryToCategoryDto(Category category)
        {
            var categoryDto = category.Adapt<CategoryDto>();
            return categoryDto;
        }
    }
}
