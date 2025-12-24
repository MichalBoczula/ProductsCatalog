using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;

namespace ProductCatalog.Application.Features.Categories.Commands.CreateCategory
{
    internal sealed record CreateCategoryCommand(CreateCategoryExternalDto Category) : IRequest<CategoryDto>;
}
