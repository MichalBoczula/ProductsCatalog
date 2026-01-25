using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.CommonDescriptions
{
    public class CommonDescriptionsDescriptionRuleTests
    {
        [Fact]
        public void IsValid_DescriptionIsNull_ShouldReturnError()
        {
            //Arrange
            var commonDescription = new CommonDescription("name", "brand", null, "photo", ["photo1"]);
            var rule = new CommonDescriptionsDescriptionRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(commonDescription, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Description cannot be null or whitespace.");
            error.Name.ShouldContain("CommonDescriptionsDescriptionRule");
            error.Entity.ShouldContain("CommonDescription");
        }

        [Fact]
        public void IsValid_DescriptionHasValue_ShouldNotReturnError()
        {
            //Arrange
            var commonDescription = new CommonDescription("name", "brand", "description", "photo", ["photo1"]);
            var rule = new CommonDescriptionsDescriptionRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(commonDescription, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }
    }
}
