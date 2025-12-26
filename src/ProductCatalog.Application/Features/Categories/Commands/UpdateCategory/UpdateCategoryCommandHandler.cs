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
            var validationResultIncoming = _validationPolicy.Validate(incoming);
            if (!validationResultIncoming.IsValid)
            {
                throw new ValidationException(validationResultIncoming);
            }

            var category = await _categoriesCommandsRepository.GetCategoryById(request.id, cancellationToken);
            var validationResultExisiting = _validationPolicy.Validate(category);
            if (!validationResultExisiting.IsValid)
            {
                throw new ValidationException(validationResultExisiting);
            }

            category.AssigneNewCategoryInformation(incoming);
            _categoriesCommandsRepository.Update(category);

            var categoriesHistory = new CategoriesHistory
            {
                CategoryId = category.Id,
                Code = category.Code,
                Name = category.Name,
                IsActive = category.IsActive,
                ChangedAt = category.ChangedAt,
                Operation = Operation.Updated
            };

            _categoriesCommandsRepository.WriteHistory(categoriesHistory);

            await _categoriesCommandsRepository.SaveChanges(cancellationToken);
            return category.Adapt<CategoryDto>();
        }
    }
}