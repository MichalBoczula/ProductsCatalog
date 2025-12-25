namespace ProductCatalog.Application.Common.Dtos.Products
{
    public sealed record MoneyDto
    {
        public required decimal Amount { get; init; }
        public required string Currency { get; init; }
    }
}
