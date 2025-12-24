using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;

namespace ProductCatalog.Application.Features.Categories.Commands.CreateCategory
{
    internal sealed class CreateCategoryCommandHandler 
        (ICategoriesCommandsRepository _categoriesCommandsRepository
         )
        : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = request.Category.Adapt<Category>();
            await _categoriesCommandsRepository.AddAsync(category, cancellationToken);
            return category.Adapt<CategoryDto>();
        }
    }
}
