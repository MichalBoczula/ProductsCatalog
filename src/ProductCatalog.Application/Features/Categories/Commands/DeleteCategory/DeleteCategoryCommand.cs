using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;

namespace ProductCatalog.Application.Features.Categories.Commands.DeleteCategory
{
    public sealed record DeleteCategoryCommand(Guid id) : IRequest<CategoryDto>;
}
