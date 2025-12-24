using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;

namespace ProductCatalog.Application.Features.Categories.Queries.GetCategoryById
{
    internal sealed class GetCategoryByIdQueryHandler
        (ICategoriesQueriesRepository _categoriesQueriesRepository) 
        : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoriesQueriesRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            return category.Adapt<CategoryDto>();
        }
    }
}
