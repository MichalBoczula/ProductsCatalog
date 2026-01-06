using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.ElectronicDetails;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.ElectronicDetails
{
    public class ElectronicDetailsNumbersValidationRuleTests
    {
        [Fact]
        public void IsValid_NumbersAreZeroOrBelow_ShouldReturnErrors()
        {
            //Arrange
            ElectronicDetails electronicDetails = default;
            var rule = new ElectronicDetailsNumbersValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(electronicDetails, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(5);
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Refresh rate cannot be zero or below."));
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Screen size cannot be zero or below."));
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Width cannot be zero or below."));
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Height cannot be zero or below."));
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Battery capacity cannot be zero or below."));
        }

        [Fact]
        public void IsValid_NumbersAreValid_ShouldNotReturnErrors()
        {
            //Arrange
            ElectronicDetails electronicDetails = new("CPU", "GPU", "RAM", "256GB", "OLED", 120, 6.5m, 70, 150, "Li-Ion", 5000);
            var rule = new ElectronicDetailsNumbersValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(electronicDetails, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRules()
        {
            //Arrange
            var rule = new ElectronicDetailsNumbersValidationRule();
            //Act
            var description = rule.Describe();
            //Assert
            description.Count.ShouldBe(5);
            description.ShouldContain(e => e.Message.Contains("Refresh rate cannot be zero or below."));
            description.ShouldContain(e => e.Message.Contains("Screen size cannot be zero or below."));
            description.ShouldContain(e => e.Message.Contains("Width cannot be zero or below."));
            description.ShouldContain(e => e.Message.Contains("Height cannot be zero or below."));
            description.ShouldContain(e => e.Message.Contains("Battery capacity cannot be zero or below."));
        }
    }
}
