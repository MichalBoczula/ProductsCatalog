using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions
{
    public sealed class CommonDescriptionsNameRule : IValidationRule<CommonDescription>
    {
        private readonly ValidationError nullOrWhiteSpace;

        public Task IsValid(CommonDescription entity, ValidationResult validationResults)
        {
            throw new NotImplementedException();
        }

        public List<ValidationError> Describe()
        {
            throw new NotImplementedException();
        }
    }
}
