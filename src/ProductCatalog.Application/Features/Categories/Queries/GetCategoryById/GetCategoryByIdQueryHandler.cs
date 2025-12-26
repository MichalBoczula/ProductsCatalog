using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Categories.Queries.GetCategoryById
{
    internal sealed class GetCategoryByIdQueryHandler
        (ICategoriesQueriesRepository _categoriesQueriesRepository)
        : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoriesQueriesRepository.GetById(request.categoryId, cancellationToken);
            if (category is null)
            {
                throw new ResourceNotFoundException(nameof(GetCategoryByIdQuery), request.categoryId, nameof(CategoryDto));
            }
            return category.Adapt<CategoryDto>();
        }
    }
}
