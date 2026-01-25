using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Concrete.Policies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Policies
{
    public class CommonDescriptionValidationPolicyTests
    {
        [Fact]
        public async Task Validate_ShouldReturnErrors()
        {
            //Arrange
            CommonDescription commonDescription = default;
            var policy = new CommonDescriptionValidationPolicy();
            //Act
            var result = await policy.Validate(commonDescription);
            //Assert
            result.GetValidatonErrors().Count.ShouldBe(4);
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CommonDescriptionsNameRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CommonDescriptionsDescriptionRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CommonDescriptionsMainPhotoRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CommonDescriptionsOtherPhotosRule");
        }

        [Fact]
        public async Task Validate_ShouldBeValid()
        {
            //Arrange
            var commonDescription = new CommonDescription("name", "brand", "description", "mainPhoto", ["photo1"]);
            var policy = new CommonDescriptionValidationPolicy();
            //Act
            var result = await policy.Validate(commonDescription);
            //Assert
            result.GetValidatonErrors().Count.ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnDescriptionForAllRoles()
        {
            //Arrange
            var policy = new CommonDescriptionValidationPolicy();
            //Act
            var result = policy.Describe();
            //Assert
            result.Rules.Count.ShouldBe(4);
            result.PolicyName.ShouldBe("CommonDescriptionValidationPolicy");
            result.Rules.ShouldContain(r => r.RuleName == "CommonDescriptionsNameRule");
            result.Rules.ShouldContain(r => r.RuleName == "CommonDescriptionsDescriptionRule");
            result.Rules.ShouldContain(r => r.RuleName == "CommonDescriptionsMainPhotoRule");
            result.Rules.ShouldContain(r => r.RuleName == "CommonDescriptionsOtherPhotosRule");
        }
    }
}
