using ProductCatalog.Domain.Validation.Concrete.Policies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Policies
{
    public class AmountValidationPolicyTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public async Task Validate_AmountIsZeroOrBelow_ShouldReturnError(int amount)
        {
            //Arrange
            var policy = new AmountValidationPolicy();
            //Act
            var result = await policy.Validate(amount);
            //Assert
            result.GetValidatonErrors().Count.ShouldBe(1);
            result.GetValidatonErrors().ShouldContain(e => e.Name == "AmountGreaterThanZeroValidationRule");
        }

        [Fact]
        public async Task Validate_AmountIsGreaterThanZero_ShouldBeValid()
        {
            //Arrange
            var policy = new AmountValidationPolicy();
            //Act
            var result = await policy.Validate(10);
            //Assert
            result.GetValidatonErrors().Count.ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnDescriptionForAllRules()
        {
            //Arrange
            var policy = new AmountValidationPolicy();
            //Act
            var result = policy.Describe();
            //Assert
            result.Rules.Count.ShouldBe(1);
            result.PolicyName.ShouldBe("AmountValidationPolicy");
            result.Rules.ShouldContain(r => r.RuleName == "AmountGreaterThanZeroValidationRule");
        }
    }
}
