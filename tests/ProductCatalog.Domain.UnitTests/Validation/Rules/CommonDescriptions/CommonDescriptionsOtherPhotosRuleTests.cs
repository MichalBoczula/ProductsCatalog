using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.CommonDescriptions
{
    public class CommonDescriptionsOtherPhotosRuleTests
    {
        [Fact]
        public void IsValid_OtherPhotosIsNull_ShouldReturnError()
        {
            //Arrange
            var commonDescription = new CommonDescription("name", "brand", "desc", "mainPhoto", null);
            var rule = new CommonDescriptionsOtherPhotosRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(commonDescription, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Other photos cannot be null or whitespace.");
            error.Name.ShouldContain("CommonDescriptionsOtherPhotosRule");
            error.Entity.ShouldContain("CommonDescription");
        }

        [Fact]
        public void IsValid_OtherPhotosContainWhitespace_ShouldReturnError()
        {
            //Arrange
            var commonDescription = new CommonDescription("name", "brand", "desc", "mainPhoto", ["photo1", " "]);
            var rule = new CommonDescriptionsOtherPhotosRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(commonDescription, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Other photos cannot be null or whitespace.");
            error.Name.ShouldContain("CommonDescriptionsOtherPhotosRule");
            error.Entity.ShouldContain("CommonDescription");
        }

        [Fact]
        public void IsValid_OtherPhotosHaveValues_ShouldNotReturnError()
        {
            //Arrange
            var commonDescription = new CommonDescription("name", "brand", "desc", "mainPhoto", ["photo1", "photo2"]);
            var rule = new CommonDescriptionsOtherPhotosRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(commonDescription, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }
    }
}
