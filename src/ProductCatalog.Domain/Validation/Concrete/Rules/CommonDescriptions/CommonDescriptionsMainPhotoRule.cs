using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions
{
    public sealed class CommonDescriptionsMainPhotoRule : IValidationRule<CommonDescription>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public CommonDescriptionsMainPhotoRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Main photo cannot be null or whitespace.",
                Name = nameof(CommonDescriptionsMainPhotoRule),
                Entity = nameof(CommonDescription)
            };
        }

        public Task IsValid(CommonDescription entity, ValidationResult validationResults)
        {
            if (string.IsNullOrWhiteSpace(entity.MainPhoto))
                validationResults.AddValidationError(nullOrWhiteSpace);

            return Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
