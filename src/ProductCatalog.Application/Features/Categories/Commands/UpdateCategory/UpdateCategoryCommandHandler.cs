using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.Categories.Commands.UpdateCategory
{
    internal sealed class UpdateCategoryCommandHandler
        (ICategoriesCommandsRepository _categoriesCommandsRepository,
         IValidationPolicy<Category> _validationPolicy,
         UpdateCategoryCommandFlowDescribtor _updateCategoryCommandFlowDescribtor) 
        : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var incoming = _updateCategoryCommandFlowDescribtor.MapRequestToCategoryAggregate(request);
            var validationResultIncoming = await _updateCategoryCommandFlowDescribtor.ValidateIncomingCategory(incoming, _validationPolicy);
            _updateCategoryCommandFlowDescribtor.ThrowValidationExceptionIfIncomingInvalid(validationResultIncoming);

            var category = await _updateCategoryCommandFlowDescribtor.LoadExistingCategory(request.id, _categoriesCommandsRepository, cancellationToken);
            var validationResultExisiting = await _updateCategoryCommandFlowDescribtor.ValidateExistingCategory(category, _validationPolicy);
            _updateCategoryCommandFlowDescribtor.ThrowValidationExceptionIfExistingInvalid(validationResultExisiting);

            _updateCategoryCommandFlowDescribtor.AssignNewCategoryInformation(category, incoming);
            _updateCategoryCommandFlowDescribtor.UpdateCategoryInRepository(category, _categoriesCommandsRepository);

            var categoriesHistory = _updateCategoryCommandFlowDescribtor.CreateCategoryHistoryEntry(category);
            _updateCategoryCommandFlowDescribtor.WriteHistoryToRepository(_categoriesCommandsRepository, categoriesHistory);

            await _updateCategoryCommandFlowDescribtor.SaveChanges(_categoriesCommandsRepository, cancellationToken);
            return _updateCategoryCommandFlowDescribtor.MapCategoryToCategoryDto(category);
        }
    }
}
