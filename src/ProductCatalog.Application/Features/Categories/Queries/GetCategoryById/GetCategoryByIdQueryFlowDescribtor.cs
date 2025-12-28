using Mapster;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Categories.Queries.GetCategoryById
{
    internal sealed class GetCategoryByIdQueryFlowDescribtor : FlowDescriberBase<GetCategoryByIdQuery>
    {
        [FlowStep(1)]
        public Task<CategoryReadModel?> GetCategory(ICategoriesQueriesRepository categoriesQueriesRepository, Guid categoryId, CancellationToken cancellationToken)
        {
            return categoriesQueriesRepository.GetById(categoryId, cancellationToken);
        }

        [FlowStep(2)]
        public CategoryReadModel EnsureCategoryFound(CategoryReadModel? category, Guid categoryId)
        {
            if (category is null)
            {
                throw new ResourceNotFoundException(nameof(GetCategoryByIdQuery), categoryId, nameof(CategoryDto));
            }

            return category;
        }

        [FlowStep(3)]
        public CategoryDto MapCategoryToDto(CategoryReadModel category)
        {
            return category.Adapt<CategoryDto>();
        }
    }
}
