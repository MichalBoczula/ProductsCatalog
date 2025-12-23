using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;

namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate
{
    public sealed class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Money Price { get; private set; }
        public bool IsActive { get; private set; }
        public Guid CategoryId { get; private set; }

        private Product() { }

        public Product(string name, string description, Money price, Guid categoryId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;

        public void ChangePrice(Money newPrice) => Price = newPrice;
    }
}
