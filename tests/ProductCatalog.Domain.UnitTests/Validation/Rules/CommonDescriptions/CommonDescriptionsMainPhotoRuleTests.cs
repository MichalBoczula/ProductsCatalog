using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.CommonDescriptions;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.CommonDescriptions
{
    public class CommonDescriptionsMainPhotoRuleTests
    {
        [Fact]
        public void IsValid_MainPhotoIsNull_ShouldReturnError()
        {
            //Arrange
            var commonDescription = new CommonDescription("name", "brand", "desc", null, ["photo1"]);
            var rule = new CommonDescriptionsMainPhotoRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(commonDescription, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Main photo cannot be null or whitespace.");
            error.Name.ShouldContain("CommonDescriptionsMainPhotoRule");
            error.Entity.ShouldContain("CommonDescription");
        }

        [Fact]
        public void IsValid_MainPhotoHasValue_ShouldNotReturnError()
        {
            //Arrange
            var commonDescription = new CommonDescription("name", "brand", "desc", "mainPhoto", ["photo1"]);
            var rule = new CommonDescriptionsMainPhotoRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(commonDescription, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }
    }
}
