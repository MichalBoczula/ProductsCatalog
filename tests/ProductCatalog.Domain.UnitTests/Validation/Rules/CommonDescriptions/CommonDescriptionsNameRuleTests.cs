using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.CommonDescriptions
{
    public class CommonDescriptionsNameRuleTests
    {
        [Fact]
        public void IsValid_NameIsNull_ShouldReturnError()
        {
            //Arrange
            var commonDescription = new CommonDescription(null, "brand", "desc", "photo", ["photo1"]);
            var rule = new CommonDescriptionsNameRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(commonDescription, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Name cannot be null or whitespace.");
            error.Name.ShouldContain("CommonDescriptionsNameRule");
            error.Entity.ShouldContain("CommonDescription");
        }

        [Fact]
        public void IsValid_NameHasValue_ShouldNotReturnError()
        {
            //Arrange
            var commonDescription = new CommonDescription("name", "brand", "desc", "photo", ["photo1"]);
            var rule = new CommonDescriptionsNameRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(commonDescription, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }
    }
}
