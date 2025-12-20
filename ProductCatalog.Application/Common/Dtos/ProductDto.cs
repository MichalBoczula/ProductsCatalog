namespace ProductCatalog.Application.Common.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public MoneyDto Price { get; init; }
        public bool IsActive { get; init; }
        public Guid CategoryId { get; init; }
    }
}
