using Mapster;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Features.Categories.Queries.GetCategories
{
    internal sealed class GetCategoriesQueryFlowDescribtor : FlowDescriberBase<GetCategoriesQuery>
    {
        [FlowStep(1)]
        public Task<IReadOnlyList<CategoryReadModel>> GetCategories(ICategoriesQueriesRepository categoriesQueriesRepository, CancellationToken cancellationToken)
        {
            return categoriesQueriesRepository.GetCategories(cancellationToken);
        }

        [FlowStep(2)]
        public IReadOnlyList<CategoryDto> MapCategoriesToDto(IReadOnlyList<CategoryReadModel> categories)
        {
            return categories.Adapt<IReadOnlyList<CategoryDto>>();
        }
    }
}
