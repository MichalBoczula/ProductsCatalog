using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Products
{
    public class ProductsCategoryIdValidationRuleTests
    {
        [Fact]
        public void IsValida_EmptyGuid_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, "usd"), Guid.Empty);
            var rule = new ProductsCategoryIdValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("CategoryId cannot be null or empty.");
            error.Name.ShouldContain("ProductsCategoryIdValidationRule");
            error.Entity.ShouldContain("Product");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new ProductsCategoryIdValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "CategoryId cannot be null or empty.",
                Name = "ProductsCategoryIdValidationRule",
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
