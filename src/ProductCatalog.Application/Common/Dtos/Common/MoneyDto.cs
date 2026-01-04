namespace ProductCatalog.Application.Common.Dtos.Common
{
    public sealed record MoneyDto
    {
        public required decimal Amount { get; init; }
        public required string Currency { get; init; }
    }
}
