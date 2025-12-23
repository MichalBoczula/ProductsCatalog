namespace ProductCatalog.Domain.ReadModels
{
    public class ProductReadModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal PriceAmount { get; init; }
        public string PriceCurrency { get; init; }
        public bool IsActive { get; init; }
        public Guid CategoryId { get; init; }
    }
}
