using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;

namespace ProductCatalog.Application.Features.Categories.Queries.GetCategoryById
{
    public sealed record GetCategoryByIdQuery(Guid categoryId) : IRequest<CategoryDto>;
}
