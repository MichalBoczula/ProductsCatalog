using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Concrete.Policies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Policies
{
    public class ElectronicDetailsValidationPolicyTests
    {
        [Fact]
        public async Task Validate_ShouldReturnErrors()
        {
            //Arrange
            ElectronicDetails electronicDetails = default;
            var policy = new ElectronicDetailsValidationPolicy();
            //Act
            var result = await policy.Validate(electronicDetails);
            //Assert
            result.GetValidatonErrors().Count.ShouldBe(11);
            result.GetValidatonErrors().ShouldContain(e => e.Name == "ElectronicDetailsStringsValidationRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "ElectronicDetailsNumbersValidationRule");
        }

        [Fact]
        public async Task Validate_ShouldBeValid()
        {
            //Arrange
            ElectronicDetails electronicDetails = new("CPU", "GPU", "RAM", "256GB", "OLED", 120, 6.5m, 70, 150, "Li-Ion", 5000);
            var policy = new ElectronicDetailsValidationPolicy();
            //Act
            var result = await policy.Validate(electronicDetails);
            //Assert
            result.GetValidatonErrors().Count.ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnDescriptionForAllRoles()
        {
            //Arrange
            var policy = new ElectronicDetailsValidationPolicy();
            //Act
            var result = policy.Describe();
            //Assert
            result.Rules.Count.ShouldBe(2);
            result.PolicyName.ShouldBe("ElectronicDetailsValidationPolicy");
            result.Rules.ShouldContain(r => r.RuleName == "ElectronicDetailsStringsValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "ElectronicDetailsNumbersValidationRule");
        }
    }
}
