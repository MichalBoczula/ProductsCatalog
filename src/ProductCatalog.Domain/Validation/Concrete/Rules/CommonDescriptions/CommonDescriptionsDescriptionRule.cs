using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions
{
    public sealed class CommonDescriptionsDescriptionRule : IValidationRule<CommonDescription>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public CommonDescriptionsDescriptionRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Description cannot be null or whitespace.",
                Name = nameof(CommonDescriptionsDescriptionRule),
                Entity = nameof(CommonDescription)
            };
        }

        public Task IsValid(CommonDescription entity, ValidationResult validationResults)
        {
            if (string.IsNullOrWhiteSpace(entity.Description))
                validationResults.AddValidationError(nullOrWhiteSpace);

            return Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
