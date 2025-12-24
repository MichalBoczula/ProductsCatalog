using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;

namespace ProductCatalog.Application.Features.Categories.Commands.DeleteCategory
{
    internal sealed class DeleteCategoryCommandHandler
        (ICategoriesCommandsRepository _categoriesCommandsRepository) 
        : IRequestHandler<DeleteCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoriesCommandsRepository.GetCategoryById(request.id, cancellationToken);
            category.Deactivate();
            await _categoriesCommandsRepository.Update(category, cancellationToken);
            return category.Adapt<CategoryDto>();
        }
    }
}
