using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Concrete.Policies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Policies
{
    public class CategoriesValidationPolicyTests
    {
        [Fact]
        public void Validate_ShouldReturnErrors()
        {
            //Arrange
            var category = new Category("", "");
            var policy = new CategoriesValidationPolicy();
            //Act
            var result = policy.Validate(category);
            //Assert
            result.GetValidatonErrors().Count().ShouldBe(2);
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CategoriesCodeValidationRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CategoriesNameValidationRule");
        }

        [Fact]
        public void Validate_CategoryIsNull_ShouldReturnErrorInRulesSet()
        {
            //Arrange
            Category category = null;
            var policy = new CategoriesValidationPolicy();
            //Act
            var result = policy.Validate(category);
            //Assert
            result.GetValidatonErrors().Count().ShouldBe(1);
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CategoryIsNullValidationRule");
        }

        [Fact]
        public void Validate_ShouldBeValid()
        {
            //Arrange
            var category = new Category("code", "name");
            var policy = new CategoriesValidationPolicy();
            //Act
            var result = policy.Validate(category);
            //Assert
            result.GetValidatonErrors().Count().ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnDescriptionForAllRoles()
        {
            //Arrange
            var policy = new CategoriesValidationPolicy();
            //Act
            var result = policy.Describe();
            //Assert
            result.Rules.Count.ShouldBe(3);
            result.PolicyName.ShouldBe("CategoriesValidationPolicy");
            result.Rules.ShouldContain(r => r.RuleName == "CategoriesCodeValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "CategoriesNameValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "CategoryIsNullValidationRule");
        }
    }
}