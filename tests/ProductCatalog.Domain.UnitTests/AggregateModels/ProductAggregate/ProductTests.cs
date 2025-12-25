using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.AggregateModels.ProductAggregate
{
    public class ProductTests
    {
        [Fact]
        public void Deactivate_IsActive_ShouldBeFalse()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, "usd"), Guid.NewGuid());
            var actualDate = product.ChangedAt;
            //Act
            product.Deactivate();
            //Assert
            product.IsActive.ShouldBeFalse();
            actualDate.ShouldBeLessThan(product.ChangedAt);
        }

        [Fact]
        public void AssigneNewProductInformation_ShouldBeCorrectlyAssigned()
        {
            //Arrange
            var oldCategoryId = Guid.NewGuid();
            var newCategoryId = Guid.NewGuid();
            var product = new Product("test", "desc", new Money(10, "usd"), oldCategoryId);
            var incoming = new Product("newName", "newDesc", new Money(15, "usd"), newCategoryId);
            var actualDate = product.ChangedAt;
            //Act
            product.AssigneNewProductInformation(incoming);
            //Assert
            product.Name.ShouldBe("newName");
            product.Description.ShouldBe("newDesc");
            product.Price.Amount.ShouldBe(15);
            product.Price.Currency.ShouldBe("usd");
            product.CategoryId.ShouldBe(newCategoryId);
            actualDate.ShouldBeLessThan(product.ChangedAt);
        }

        [Fact]
        public void SetChangeDate_ShouldDateBeAssigned()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, "usd"), Guid.NewGuid());
            var actualDate = product.ChangedAt;
            //Act
            product.SetChangeDate();
            //Assert
            actualDate.ShouldBeLessThan(product.ChangedAt);
        }

        [Fact]
        public void CreateAggreageteWithId_ShouldCreateId()
        {
            //Arrange & Act
            var product = new Product("test", "desc", new Money(10, "usd"), Guid.NewGuid());
            //Assert
            product.Id.ShouldNotBe(Guid.Empty);
        }
    }
}
