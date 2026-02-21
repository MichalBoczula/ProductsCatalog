using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Common;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Common
{
    public class AmountGreaterThanZeroValidationRuleTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task IsValid_AmountIsZeroOrBelow_ShouldReturnError(int amount)
        {
            //Arrange
            var rule = new AmountGreaterThanZeroValidationRule();
            var validationResult = new ValidationResult();
            //Act
            await rule.IsValid(amount, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Amount must be greater than zero.");
            error.Name.ShouldContain("AmountGreaterThanZeroValidationRule");
            error.Entity.ShouldContain("Amount");
        }

        [Fact]
        public async Task IsValid_AmountIsGreaterThanZero_ShouldNotReturnError()
        {
            //Arrange
            var rule = new AmountGreaterThanZeroValidationRule();
            var validationResult = new ValidationResult();
            //Act
            await rule.IsValid(1, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new AmountGreaterThanZeroValidationRule();
            //Act
            var result = rule.Describe();
            //Assert
            result.Count.ShouldBe(1);
            var desc = result.First();
            desc.Message.ShouldBe("Amount must be greater than zero.");
            desc.Name.ShouldBe("AmountGreaterThanZeroValidationRule");
            desc.Entity.ShouldBe("Amount");
        }
    }
}
