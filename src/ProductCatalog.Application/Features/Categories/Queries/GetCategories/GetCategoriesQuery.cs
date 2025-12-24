using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;

namespace ProductCatalog.Application.Features.Categories.Queries.GetCategories
{
    public sealed record GetCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>;
}
