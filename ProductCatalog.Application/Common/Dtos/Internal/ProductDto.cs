using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;

namespace ProductCatalog.Application.Common.Dtos.Internal
{
    internal class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Money Price { get; set; }
        public bool IsActive { get; set; }
        public Guid CategoryId { get; set; }
    }
}
