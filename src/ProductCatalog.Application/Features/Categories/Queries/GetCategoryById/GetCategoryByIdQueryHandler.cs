using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Categories.Queries.GetCategoryById
{
    internal sealed class GetCategoryByIdQueryHandler
        (ICategoriesQueriesRepository _categoriesQueriesRepository,
         GetCategoryByIdQueryFlowDescribtor _getCategoryByIdQueryFlowDescribtor)
        : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _getCategoryByIdQueryFlowDescribtor.GetCategory(_categoriesQueriesRepository, request.categoryId, cancellationToken);
            var existingCategory = _getCategoryByIdQueryFlowDescribtor.EnsureCategoryFound(category, request.categoryId);
            return _getCategoryByIdQueryFlowDescribtor.MapCategoryToDto(existingCategory);
        }
    }
}
