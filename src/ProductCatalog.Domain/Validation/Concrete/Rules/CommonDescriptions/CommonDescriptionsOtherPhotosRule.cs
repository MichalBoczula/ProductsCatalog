using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions
{
    public sealed class CommonDescriptionsOtherPhotosRule : IValidationRule<CommonDescription>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public CommonDescriptionsOtherPhotosRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Other photos cannot be null or whitespace.",
                Name = nameof(CommonDescriptionsOtherPhotosRule),
                Entity = nameof(CommonDescription)
            };
        }

        public Task IsValid(CommonDescription entity, ValidationResult validationResults)
        {
            if (entity.OtherPhotos is null || entity.OtherPhotos.Any(string.IsNullOrWhiteSpace))
                validationResults.AddValidationError(nullOrWhiteSpace);

            return Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
