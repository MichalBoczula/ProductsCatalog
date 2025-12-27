using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
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
            var validationResult = await _validationPolicy.Validate(category);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            _categoriesCommandsRepository.Add(category);

            var categoriesHistory = category.BuildAdapter()
               .AddParameters("operation", Operation.Inserted)
               .AdaptToType<CategoriesHistory>();

            _categoriesCommandsRepository.WriteHistory(categoriesHistory);

            await _categoriesCommandsRepository.SaveChanges(cancellationToken);
            return category.Adapt<CategoryDto>();
        }
    }
}
