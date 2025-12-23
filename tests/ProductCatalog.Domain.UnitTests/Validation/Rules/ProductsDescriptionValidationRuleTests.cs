using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules
{
    public class ProductsDescriptionValidationRuleTests
    {
        [Fact]
        public void IsValid_DescriptionIsEmpty_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", "", new Money(10, "usd"), Guid.NewGuid());
            var rule = new ProductsDescriptionValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidateErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidateErrors().First();
            error.Message.ShouldContain("Descriptions cannot be null or whitespace.");
            error.Name.ShouldContain("DescriptionsIsNullOrWhiteSpace");
        }

        [Fact]
        public void IsValid_DescriptionIsNull_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", null, new Money(10, "usd"), Guid.NewGuid());
            var rule = new ProductsDescriptionValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidateErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidateErrors().First();
            error.Message.ShouldContain("Descriptions cannot be null or whitespace.");
            error.Name.ShouldContain("DescriptionsIsNullOrWhiteSpace");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new ProductsDescriptionValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Descriptions cannot be null or whitespace.",
                Name = "DescriptionsIsNullOrWhiteSpace",
            };

            //Act
            var result = rule.Describe();
            //Assert
            result.Count.ShouldBe(1);
            var desc = result.First();
            desc.Message.ShouldBe(nullOrEmpty.Message);
            desc.Name.ShouldBe(nullOrEmpty.Name);
        }
    }
}
