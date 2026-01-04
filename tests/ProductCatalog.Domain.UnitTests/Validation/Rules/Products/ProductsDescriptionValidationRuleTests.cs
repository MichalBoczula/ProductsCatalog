using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Products
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
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Description cannot be null or whitespace.");
            error.Name.ShouldContain("ProductsDescriptionValidationRule");
            error.Entity.ShouldContain("Product");
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
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Description cannot be null or whitespace.");
            error.Name.ShouldContain("ProductsDescriptionValidationRule");
            error.Entity.ShouldContain("Product");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new ProductsDescriptionValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Description cannot be null or whitespace.",
                Name = "ProductsDescriptionValidationRule",
                Entity = nameof(Product)
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
