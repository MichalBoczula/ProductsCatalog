using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;

namespace ProductCatalog.Application.Features.Categories.Commands.CreateCategory
{
    public sealed record CreateCategoryCommand(CreateCategoryExternalDto category) : IRequest<CategoryDto>;
}
