using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.ElectronicDetails
{
    public sealed class ElectronicDetailsStringsValidationRule : IValidationRule<ElectronicDetails>
    {
        private readonly ValidationError cpuIsNullOrWhitespace;
        private readonly ValidationError gpuIsNullOrWhitespace;
        private readonly ValidationError ramIsNullOrWhitespace;
        private readonly ValidationError storageIsNullOrWhitespace;
        private readonly ValidationError displayTypeIsNullOrWhitespace;
        private readonly ValidationError batteryTypeIsNullOrWhitespace;

        public ElectronicDetailsStringsValidationRule()
        {
            cpuIsNullOrWhitespace = new ValidationError
            {
                Message = "CPU cannot be null or whitespace.",
                Name = nameof(ElectronicDetailsStringsValidationRule),
                Entity = nameof(ElectronicDetails)
            };

            gpuIsNullOrWhitespace = new ValidationError
            {
                Message = "GPU cannot be null or whitespace.",
                Name = nameof(ElectronicDetailsStringsValidationRule),
                Entity = nameof(ElectronicDetails)
            };

            ramIsNullOrWhitespace = new ValidationError
            {
                Message = "Ram cannot be null or whitespace.",
                Name = nameof(ElectronicDetailsStringsValidationRule),
                Entity = nameof(ElectronicDetails)
            };

            storageIsNullOrWhitespace = new ValidationError
            {
                Message = "Storage cannot be null or whitespace.",
                Name = nameof(ElectronicDetailsStringsValidationRule),
                Entity = nameof(ElectronicDetails)
            };

            displayTypeIsNullOrWhitespace = new ValidationError
            {
                Message = "Display type cannot be null or whitespace.",
                Name = nameof(ElectronicDetailsStringsValidationRule),
                Entity = nameof(ElectronicDetails)
            };

            batteryTypeIsNullOrWhitespace = new ValidationError
            {
                Message = "Battery type cannot be null or whitespace.",
                Name = nameof(ElectronicDetailsStringsValidationRule),
                Entity = nameof(ElectronicDetails)
            };
        }

        public Task IsValid(ElectronicDetails entity, ValidationResult validationResults)
        {
            if (string.IsNullOrWhiteSpace(entity.CPU))
                validationResults.AddValidationError(cpuIsNullOrWhitespace);

            if (string.IsNullOrWhiteSpace(entity.GPU))
                validationResults.AddValidationError(gpuIsNullOrWhitespace);

            if (string.IsNullOrWhiteSpace(entity.Ram))
                validationResults.AddValidationError(ramIsNullOrWhitespace);

            if (string.IsNullOrWhiteSpace(entity.Storage))
                validationResults.AddValidationError(storageIsNullOrWhitespace);

            if (string.IsNullOrWhiteSpace(entity.DisplayType))
                validationResults.AddValidationError(displayTypeIsNullOrWhitespace);

            if (string.IsNullOrWhiteSpace(entity.BatteryType))
                validationResults.AddValidationError(batteryTypeIsNullOrWhitespace);

            return Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return
            [
                cpuIsNullOrWhitespace,
                gpuIsNullOrWhitespace,
                ramIsNullOrWhitespace,
                storageIsNullOrWhitespace,
                displayTypeIsNullOrWhitespace,
                batteryTypeIsNullOrWhitespace
            ];
        }
    }
}
