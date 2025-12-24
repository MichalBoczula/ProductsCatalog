using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;

namespace ProductCatalog.Application.Features.Categories.Commands.UpdateCategory
{
    public sealed record UpdateCategoryCommand(Guid id, UpdateCategoryExternalDto category) : IRequest<CategoryDto>;
}
