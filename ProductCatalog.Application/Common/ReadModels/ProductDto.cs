using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;

namespace ProductCatalog.Application.Common.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MoneyDto Price { get; set; }
        public bool IsActive { get; set; }
        public Guid CategoryId { get; set; }
    }
}
