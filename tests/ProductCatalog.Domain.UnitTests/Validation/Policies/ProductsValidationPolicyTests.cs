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
            var product = new Product("", "", new Money(10, "usd"), Guid.Empty);
            var policy = new ProductsValidationPolicy();
            //Act
            var result = policy.Validate(product);
            //Assert
            result.GetValidateErrors().Count().ShouldBe(3);
            result.GetValidateErrors().ShouldContain(e => e.Name == "ProductsNameIsNullOrWhiteSpace");
            result.GetValidateErrors().ShouldContain(e => e.Name == "DescriptionsIsNullOrWhiteSpace");
            result.GetValidateErrors().ShouldContain(e => e.Name == "CategoryIdIsNullOrWhiteSpace");
        }

        [Fact]
        public void Validate_ValidObject_ShouldReturnEmptyRulesSet()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, "usd"), Guid.NewGuid());
            var policy = new ProductsValidationPolicy();
            //Act
            var result = policy.Validate(product);
            //Assert
            result.GetValidateErrors().Count().ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnDescriptionForAllRoles()
        {
            //Arrange
            var policy = new ProductsValidationPolicy();
            //Act
            var result = policy.Describe();
            //Assert
            result.Rules.Count.ShouldBe(3);
            result.PolicyName.ShouldBe("ProductsValidationPolicy");
            result.Rules.ShouldContain(r => r.RuleName == "ProductsNameValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "ProductsDescriptionValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "ProductsCategoryIdValidationRule");
        }
    }
}