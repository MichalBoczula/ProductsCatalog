using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.Categories.Commands.CreateCategory
{
    internal sealed class CreateCategoryCommandHandler
        (ICategoriesCommandsRepository _categoriesCommandsRepository,
         IValidationPolicy<Category> _validationPolicy,
         CreateCategoryCommandFlowDescribtor _createCategoryCommandFlowDescribtor)
        : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _createCategoryCommandFlowDescribtor.MapRequestToCategoryAggregate(request);
            var validationResult = await _createCategoryCommandFlowDescribtor.ValidateCategoryAggregate(category, _validationPolicy);
            _createCategoryCommandFlowDescribtor.ThrowValidationExceptionIfNotValid(validationResult);

            _createCategoryCommandFlowDescribtor.AddCategoryToRepository(category, _categoriesCommandsRepository);

            var categoriesHistory = _createCategoryCommandFlowDescribtor.CreateCategoryHistoryEntry(category);
            _createCategoryCommandFlowDescribtor.WriteHistoryToRepository(_categoriesCommandsRepository, categoriesHistory);

            await _createCategoryCommandFlowDescribtor.SaveChanges(_categoriesCommandsRepository, cancellationToken);
            return _createCategoryCommandFlowDescribtor.MapCategoryToCategoryDto(category);
        }
    }
}
