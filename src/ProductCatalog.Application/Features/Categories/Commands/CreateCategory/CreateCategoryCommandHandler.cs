using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Categories.Commands.CreateCategory
{
    internal sealed class CreateCategoryCommandHandler
        (ICategoriesCommandsRepository _categoriesCommandsRepository,
         IValidationPolicy<Category> _validationPolicy)
        : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = request.category.Adapt<Category>();
            var validationResult = _validationPolicy.Validate(category);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            await _categoriesCommandsRepository.AddAsync(category, cancellationToken);
            return category.Adapt<CategoryDto>();
        }
    }
}
