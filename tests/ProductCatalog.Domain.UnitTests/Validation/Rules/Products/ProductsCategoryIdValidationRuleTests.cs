using Moq;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Products
{
    public class ProductsCategoryIdValidationRuleTests
    {
        [Fact]
        public async Task IsValid_EmptyGuid_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, "usd"), Guid.Empty);
            var categoriesQueriesRepository = new Mock<ICategoriesQueriesRepository>();

            var rule = new ProductsCategoryIdValidationRule(categoriesQueriesRepository.Object);
            var validationResult = new ValidationResult();
            //Act
            await rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(2);
            var error = validationResult.GetValidatonErrors().First();
            var error2 = validationResult.GetValidatonErrors().Last();
            error.Message.ShouldContain("CategoryId cannot be null or empty.");
            error.Name.ShouldContain("ProductsCategoryIdValidationRule");
            error.Entity.ShouldContain("Product");
            error2.Message.ShouldContain("CategoryId does not exist.");
            error2.Name.ShouldContain("ProductsCategoryIdValidationRule");
            error2.Entity.ShouldContain("Product");
            categoriesQueriesRepository.Verify(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task IsValid_CategoryIdNotExists_ShouldReturnError()
        {
            //Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product("test", "desc", new Money(10, "usd"), Guid.NewGuid());
            var categoriesQueriesRepository = new Mock<ICategoriesQueriesRepository>();
            var categoryReadModels = new CategoryReadModel
            {
                Id = categoryId,
                Code = "USD",
                Name = "US Dollar",
                IsActive = true
            };

            categoriesQueriesRepository
                .Setup(r => r.GetById(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(categoryReadModels);

            var rule = new ProductsCategoryIdValidationRule(categoriesQueriesRepository.Object);
            var validationResult = new ValidationResult();
            //Act
            await rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("CategoryId does not exist.");
            error.Name.ShouldContain("ProductsCategoryIdValidationRule");
            error.Entity.ShouldContain("Product");
            categoriesQueriesRepository.Verify(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task IsValid_ShouldBeValid()
        {
            //Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product("test", "desc", new Money(10, "usd"), categoryId);
            var categoriesQueriesRepository = new Mock<ICategoriesQueriesRepository>();
            var categoryReadModels = new CategoryReadModel
            {
                Id = categoryId,
                Code = "USD",
                Name = "US Dollar",
                IsActive = true
            };

            categoriesQueriesRepository
                .Setup(r => r.GetById(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(categoryReadModels);

            var rule = new ProductsCategoryIdValidationRule(categoriesQueriesRepository.Object);
            var validationResult = new ValidationResult();
            //Act
            await rule.IsValid(product, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(0);
            categoriesQueriesRepository.Verify(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var categoriesQueriesRepository = new Mock<ICategoriesQueriesRepository>();
            var rule = new ProductsCategoryIdValidationRule(categoriesQueriesRepository.Object);
            var nullOrEmpty = new ValidationError
            {
                Message = "CategoryId cannot be null or empty.",
                Name = "ProductsCategoryIdValidationRule",
                Entity = nameof(Product)
            };

            var categoryNotExists = new ValidationError
            {
                Message = "CategoryId does not exist.",
                Name = nameof(ProductsCategoryIdValidationRule),
                Entity = nameof(Product)
            };

            //Act
            var result = rule.Describe();
            //Assert
            result.Count.ShouldBe(2);
            var desc = result.First();
            var desc2 = result.Last();
            desc.Message.ShouldBe(nullOrEmpty.Message);
            desc.Name.ShouldBe(nullOrEmpty.Name);
            desc.Entity.ShouldBe(nullOrEmpty.Entity);
            desc2.Message.ShouldBe(categoryNotExists.Message);
            desc2.Name.ShouldBe(categoryNotExists.Name);
            desc2.Entity.ShouldBe(categoryNotExists.Entity);
        }
    }
}
