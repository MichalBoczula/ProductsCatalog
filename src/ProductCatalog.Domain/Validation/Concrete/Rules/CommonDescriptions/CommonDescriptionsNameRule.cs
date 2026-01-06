using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions
{
    public sealed class CommonDescriptionsNameRule : IValidationRule<CommonDescription>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public CommonDescriptionsNameRule()
        {
            nullOrWhiteSpace = new ValidationError
            {
                Message = "Name cannot be null or whitespace.",
                Name = nameof(CommonDescriptionsNameRule),
                Entity = nameof(CommonDescription)
            };
        }

        public Task IsValid(CommonDescription entity, ValidationResult validationResults)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                validationResults.AddValidationError(nullOrWhiteSpace);

            return Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return [nullOrWhiteSpace];
        }
    }
}
