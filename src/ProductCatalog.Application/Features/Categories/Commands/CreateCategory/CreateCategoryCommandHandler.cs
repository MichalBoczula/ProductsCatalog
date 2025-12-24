using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;

namespace ProductCatalog.Application.Features.Categories.Commands.CreateCategory
{
    internal sealed class CreateCategoryCommandHandler 
        (
         )
        : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        public Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
