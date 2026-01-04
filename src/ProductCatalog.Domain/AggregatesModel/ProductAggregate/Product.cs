using ProductCatalog.Domain.AggregatesModel.Common;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;

namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate
{
    public sealed class Product : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Money Price { get; private set; }
        public Guid CategoryId { get; private set; }

        private Product() { }

        public Product(
            string name,
            string description,
            Money price,
            Guid categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
        }

        public void AssigneNewProductInformation(Product incoming)
        {
            Name = incoming.Name;
            Description = incoming.Description;
            Price = incoming.Price;
            CategoryId = incoming.CategoryId;
            this.SetChangeDate();
        }
    }
}
