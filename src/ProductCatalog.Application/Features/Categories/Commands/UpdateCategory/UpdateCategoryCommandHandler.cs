using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;

namespace ProductCatalog.Application.Features.Categories.Commands.UpdateCategory
{
    internal sealed class UpdateCategoryCommandHandler
        (ICategoriesCommandsRepository _categoriesCommandsRepository
        ) 
        : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoriesCommandsRepository.GetCategoryById(request.id, cancellationToken);
            var incoming = request.category.Adapt<Category>();
            category.AssigneNewCategoryInformation(incoming);
            await _categoriesCommandsRepository.Update(category, cancellationToken);
            return category.Adapt<CategoryDto>();
        }
    }
}