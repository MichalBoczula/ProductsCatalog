using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Categories.Commands.DeleteCategory
{
    internal sealed class DeleteCategoryCommandHandler
        (ICategoriesCommandsRepository _categoriesCommandsRepository,
         IValidationPolicy<Category> _validationPolicy)
        : IRequestHandler<DeleteCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoriesCommandsRepository.GetCategoryById(request.id, cancellationToken);
            var validationResult = await _validationPolicy.Validate(category);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            category.Deactivate();
            _categoriesCommandsRepository.Update(category);

            var categoriesHistory = category.BuildAdapter()
               .AddParameters("operation", Operation.Deleted)
               .AdaptToType<CategoriesHistory>();

            _categoriesCommandsRepository.WriteHistory(categoriesHistory);
           
            await _categoriesCommandsRepository.SaveChanges(cancellationToken);
            return category.Adapt<CategoryDto>();
        }
    }
}
