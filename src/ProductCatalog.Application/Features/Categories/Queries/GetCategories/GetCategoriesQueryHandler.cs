using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;

namespace ProductCatalog.Application.Features.Categories.Queries.GetCategories
{
    internal sealed class GetCategoriesQueryHandler
        (ICategoriesQueriesRepository _categoriesQueriesRepository,
         GetCategoriesQueryFlowDescribtor _getCategoriesQueryFlowDescribtor)
        : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>
    {
        public async Task<IReadOnlyList<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _getCategoriesQueryFlowDescribtor.GetCategories(_categoriesQueriesRepository, cancellationToken);
            return _getCategoriesQueryFlowDescribtor.MapCategoriesToDto(categories);
        }
    }
}
