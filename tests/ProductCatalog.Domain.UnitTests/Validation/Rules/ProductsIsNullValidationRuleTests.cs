using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules
{
    public class ProductsIsNullValidationRuleTests
    {
        [Fact]
        public void IsValid_ProductIsNull_ShouldReturnError()
        {
            //Arrange
            Product product = null;
            var rule = new ProductsIsNullValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidateErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidateErrors().First();
            error.Message.ShouldContain("Product cannot be null.");
            error.Name.ShouldContain("ProductIsNull");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new ProductsIsNullValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Product cannot be null.",
                Name = "ProductIsNull",
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
