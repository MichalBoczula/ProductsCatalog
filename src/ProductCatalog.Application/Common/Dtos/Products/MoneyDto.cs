namespace ProductCatalog.Application.Common.Dtos.Products
{
    public sealed record MoneyDto
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; }
    }
}
