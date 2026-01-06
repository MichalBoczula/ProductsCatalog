using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.ElectronicDetailsRules
{
    public sealed class ElectronicDetailsNumbersValidationRule : IValidationRule<ElectronicDetails>
    {
        private readonly ValidationError refreshRateIsZeroOrBelow;
        private readonly ValidationError screenSizeIsZeroOrBelow;
        private readonly ValidationError widthIsZeroOrBelow;
        private readonly ValidationError heightIsZeroOrBelow;
        private readonly ValidationError batteryCapacityIsZeroOrBelow;

        public ElectronicDetailsNumbersValidationRule()
        {
            refreshRateIsZeroOrBelow = new ValidationError
            {
                Message = "Refresh rate cannot be zero or below.",
                Name = nameof(ElectronicDetailsNumbersValidationRule),
                Entity = nameof(ElectronicDetails)
            };

            screenSizeIsZeroOrBelow = new ValidationError
            {
                Message = "Screen size cannot be zero or below.",
                Name = nameof(ElectronicDetailsNumbersValidationRule),
                Entity = nameof(ElectronicDetails)
            };

            widthIsZeroOrBelow = new ValidationError
            {
                Message = "Width cannot be zero or below.",
                Name = nameof(ElectronicDetailsNumbersValidationRule),
                Entity = nameof(ElectronicDetails)
            };

            heightIsZeroOrBelow = new ValidationError
            {
                Message = "Height cannot be zero or below.",
                Name = nameof(ElectronicDetailsNumbersValidationRule),
                Entity = nameof(ElectronicDetails)
            };

            batteryCapacityIsZeroOrBelow = new ValidationError
            {
                Message = "Battery capacity cannot be zero or below.",
                Name = nameof(ElectronicDetailsNumbersValidationRule),
                Entity = nameof(ElectronicDetails)
            };
        }

        public Task IsValid(ElectronicDetails entity, ValidationResult validationResults)
        {
            if (entity.RefreshRateHz <= 0)
                validationResults.AddValidationError(refreshRateIsZeroOrBelow);

            if (entity.ScreenSizeInches <= 0)
                validationResults.AddValidationError(screenSizeIsZeroOrBelow);

            if (entity.Width <= 0)
                validationResults.AddValidationError(widthIsZeroOrBelow);

            if (entity.Height <= 0)
                validationResults.AddValidationError(heightIsZeroOrBelow);

            if (entity.BatteryCapacity <= 0)
                validationResults.AddValidationError(batteryCapacityIsZeroOrBelow);

            return Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return
            [
                refreshRateIsZeroOrBelow,
                screenSizeIsZeroOrBelow,
                widthIsZeroOrBelow,
                heightIsZeroOrBelow,
                batteryCapacityIsZeroOrBelow
            ];
        }
    }
}
