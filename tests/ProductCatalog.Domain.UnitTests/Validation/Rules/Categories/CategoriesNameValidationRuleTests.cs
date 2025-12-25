using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Categories;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Categories
{
    public class CategoriesNameValidationRuleTests
    {
        [Fact]
        public void IsValid_NameIsEmpty_ShouldReturnError()
        {
            //Arrange
            var product = new Category("code", "");
            var rule = new CategoriesNameValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Name cannot be null or whitespace.");
            error.Name.ShouldContain("CategoriesNameValidationRule");
            error.Entity.ShouldContain("Category");
        }

        [Fact]
        public void IsValid_NameIsNull_ShouldReturnError()
        {
            //Arrange
            var product = new Category("code", null);
            var rule = new CategoriesNameValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Name cannot be null or whitespace.");
            error.Name.ShouldContain("CategoriesNameValidationRule");
            error.Entity.ShouldContain("Category");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new CategoriesNameValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Name cannot be null or whitespace.",
                Name = "CategoriesNameValidationRule",
                Entity = nameof(Category)
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
