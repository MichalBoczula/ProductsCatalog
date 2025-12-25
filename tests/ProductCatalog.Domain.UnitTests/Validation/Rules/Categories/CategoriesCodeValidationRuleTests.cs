using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Categories;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Categories
{
    public class CategoriesCodeValidationRuleTests
    {
        [Fact]
        public void IsValid_CodeIsEmpty_ShouldReturnError()
        {
            //Arrange
            var product = new Category("", "name");
            var rule = new CategoriesCodeValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Categories code cannot be null or whitespace.");
            error.Name.ShouldContain("CategoriesCodeIsNullOrWhiteSpace");
        }

        [Fact]
        public void IsValid_CodeIsNull_ShouldReturnError()
        {
            //Arrange
            var product = new Category(null, "name");
            var rule = new CategoriesCodeValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Categories code cannot be null or whitespace.");
            error.Name.ShouldContain("CategoriesCodeIsNullOrWhiteSpace");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new CategoriesCodeValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Categories code cannot be null or whitespace.",
                Name = "CategoriesCodeIsNullOrWhiteSpace",
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