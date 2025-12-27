using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Categories.Commands.UpdateCategory
{
    internal sealed class UpdateCategoryCommandHandler
        (ICategoriesCommandsRepository _categoriesCommandsRepository,
         IValidationPolicy<Category> _validationPolicy) 
        : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var incoming = request.category.Adapt<Category>();
            var validationResultIncoming = await _validationPolicy.Validate(incoming);
            if (!validationResultIncoming.IsValid)
            {
                throw new ValidationException(validationResultIncoming);
            }

            var category = await _categoriesCommandsRepository.GetCategoryById(request.id, cancellationToken);
            var validationResultExisiting = await _validationPolicy.Validate(category);
            if (!validationResultExisiting.IsValid)
            {
                throw new ValidationException(validationResultExisiting);
            }

            category.AssigneNewCategoryInformation(incoming);
            _categoriesCommandsRepository.Update(category);

            var categoriesHistory = category.BuildAdapter()
               .AddParameters("operation", Operation.Updated)
               .AdaptToType<CategoriesHistory>();

            _categoriesCommandsRepository.WriteHistory(categoriesHistory);

            await _categoriesCommandsRepository.SaveChanges(cancellationToken);
            return category.Adapt<CategoryDto>();
        }
    }
}