using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Products
{
    public class ProductsNameValidationRuleTests
    {
        [Fact]
        public void IsValid_NameIsEmpty_ShouldReturnError()
        {
            //Arrange
            var product = new Product("", "desc", new Money(10, "usd"), Guid.NewGuid());
            var rule = new ProductsNameValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Name cannot be null or whitespace.");
            error.Name.ShouldContain("ProductsNameValidationRule");
            error.Entity.ShouldContain("Product");
        }

        [Fact]
        public void IsValid_NameIsNull_ShouldReturnError()
        {
            //Arrange
            var product = new Product(null, "desc", new Money(10, "usd"), Guid.NewGuid());
            var rule = new ProductsNameValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Name cannot be null or whitespace.");
            error.Name.ShouldContain("ProductsNameValidationRule");
            error.Entity.ShouldContain("Product");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new ProductsNameValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Name cannot be null or whitespace.",
                Name = "ProductsNameValidationRule",
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
