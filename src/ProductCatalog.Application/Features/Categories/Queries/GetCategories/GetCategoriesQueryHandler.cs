using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;

namespace ProductCatalog.Application.Features.Categories.Queries.GetCategories
{
    internal sealed class GetCategoriesQueryHandler
        (ICategoriesQueriesRepository _categoriesQueriesRepository)
        : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>
    {
        public async Task<IReadOnlyList<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoriesQueriesRepository.GetCategories(cancellationToken);
            return category.Adapt<IReadOnlyList<CategoryDto>>();
        }
    }
}