using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Categories;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Categories
{
    public class CategoryIsNullValidationRuleTests
    {
        [Fact]
        public void IsValid_CategoryIsNull_ShouldReturnError()
        {
            //Arrange
            Category product = null;
            var rule = new CategoryIsNullValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Category cannot be null.");
            error.Name.ShouldContain("CategoryIsNullValidationRule");
            error.Entity.ShouldContain("Category");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new CategoryIsNullValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Category cannot be null.",
                Name = "CategoryIsNullValidationRule",
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
