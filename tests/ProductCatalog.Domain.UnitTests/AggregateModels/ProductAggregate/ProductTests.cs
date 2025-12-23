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
            //Act
            product.Deactivate();
            //Assert
            product.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void ChangePrice_Price_ShouldBeEqualToNext()
        {
            //Arrange
            var prev = new Money(10, "usd");
            var next = new Money(20, "pln");
            var product = new Product("test", "desc", prev, Guid.NewGuid());
            //Act
            product.ChangePrice(next);
            //Assert
            product.Price.ShouldBe(next);
        }
    }
}
