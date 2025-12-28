using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.Categories.Commands.DeleteCategory
{
    internal sealed class DeleteCategoryCommandHandler
        (ICategoriesCommandsRepository _categoriesCommandsRepository,
         IValidationPolicy<Category> _validationPolicy,
         DeleteCategoryCommandFlowDescribtor _deleteCategoryCommandFlowDescribtor)
        : IRequestHandler<DeleteCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _deleteCategoryCommandFlowDescribtor.LoadCategory(request.id, _categoriesCommandsRepository, cancellationToken);
            var validationResult = await _deleteCategoryCommandFlowDescribtor.ValidateCategory(category, _validationPolicy);
            _deleteCategoryCommandFlowDescribtor.ThrowValidationExceptionIfNotValid(validationResult);

            _deleteCategoryCommandFlowDescribtor.DeactivateCategory(category);
            _deleteCategoryCommandFlowDescribtor.UpdateCategoryInRepository(category, _categoriesCommandsRepository);

            var categoriesHistory = _deleteCategoryCommandFlowDescribtor.CreateCategoryHistoryEntry(category);
            _deleteCategoryCommandFlowDescribtor.WriteHistoryToRepository(_categoriesCommandsRepository, categoriesHistory);

            await _deleteCategoryCommandFlowDescribtor.SaveChanges(_categoriesCommandsRepository, cancellationToken);
            return _deleteCategoryCommandFlowDescribtor.MapCategoryToCategoryDto(category);
        }
    }
}
