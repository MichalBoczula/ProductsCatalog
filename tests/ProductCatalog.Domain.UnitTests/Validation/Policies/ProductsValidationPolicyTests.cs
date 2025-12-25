using ProductCatalog.Domain.Validation.Concrete.Policies;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Policies
{
    public class ProductsValidationPolicyTests
    {
        [Fact]
        public void Validate_InValidObject_ShouldReturnWholeRulesSet()
        {
            //Arrange
            var product = new Product("", "", new Money(), Guid.Empty);
            var policy = new ProductsValidationPolicy();
            //Act
            var result = policy.Validate(product);
            //Assert
            result.GetValidatonErrors().Count().ShouldBe(5);
            result.GetValidatonErrors().ShouldContain(e => e.Name == "ProductsNameValidationRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "ProductsDescriptionValidationRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "ProductsCategoryIdValidationRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "MoneyAmountValidationRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "MoneyCurrencyValidationRule");
        }

        [Fact]
        public void Validate_ProductIsNull_ShouldReturnErrorInRulesSet()
        {
            //Arrange
            Product product = null;
            var policy = new ProductsValidationPolicy();
            //Act
            var result = policy.Validate(product);
            //Assert
            result.GetValidatonErrors().Count().ShouldBe(1);
            result.GetValidatonErrors().ShouldContain(e => e.Name == "ProductsIsNullValidationRule");
        }

        [Fact]
        public void Validate_ShouldBeValid()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, "usd"), Guid.NewGuid());
            var policy = new ProductsValidationPolicy();
            //Act
            var result = policy.Validate(product);
            //Assert
            result.GetValidatonErrors().Count().ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnDescriptionForAllRoles()
        {
            //Arrange
            var policy = new ProductsValidationPolicy();
            //Act
            var result = policy.Describe();
            //Assert
            result.Rules.Count.ShouldBe(6);
            result.PolicyName.ShouldBe("ProductsValidationPolicy");
            result.Rules.ShouldContain(r => r.RuleName == "ProductsNameValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "ProductsDescriptionValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "ProductsCategoryIdValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "ProductsIsNullValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "MoneyAmountValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "MoneyCurrencyValidationRule");
        }
    }
}