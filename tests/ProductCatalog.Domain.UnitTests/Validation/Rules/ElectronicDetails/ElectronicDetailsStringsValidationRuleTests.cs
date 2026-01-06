using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.ElectronicDetails;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.ElectronicDetails
{
    public class ElectronicDetailsStringsValidationRuleTests
    {
        [Fact]
        public void IsValid_StringsAreNullOrWhitespace_ShouldReturnErrors()
        {
            //Arrange
            ElectronicDetails electronicDetails = default;
            var rule = new ElectronicDetailsStringsValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(electronicDetails, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(6);
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("CPU cannot be null or whitespace."));
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("GPU cannot be null or whitespace."));
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Ram cannot be null or whitespace."));
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Storage cannot be null or whitespace."));
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Display type cannot be null or whitespace."));
            validationResult.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Battery type cannot be null or whitespace."));
        }

        [Fact]
        public void IsValid_StringsAreValid_ShouldNotReturnErrors()
        {
            //Arrange
            ElectronicDetails electronicDetails = new("CPU", "GPU", "RAM", "256GB", "OLED", 120, 6.5m, 70, 150, "Li-Ion", 5000);
            var rule = new ElectronicDetailsStringsValidationRule();
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
            var rule = new ElectronicDetailsStringsValidationRule();
            //Act
            var description = rule.Describe();
            //Assert
            description.Count.ShouldBe(6);
            description.ShouldContain(e => e.Message.Contains("CPU cannot be null or whitespace."));
            description.ShouldContain(e => e.Message.Contains("GPU cannot be null or whitespace."));
            description.ShouldContain(e => e.Message.Contains("Ram cannot be null or whitespace."));
            description.ShouldContain(e => e.Message.Contains("Storage cannot be null or whitespace."));
            description.ShouldContain(e => e.Message.Contains("Display type cannot be null or whitespace."));
            description.ShouldContain(e => e.Message.Contains("Battery type cannot be null or whitespace."));
        }
    }
}
